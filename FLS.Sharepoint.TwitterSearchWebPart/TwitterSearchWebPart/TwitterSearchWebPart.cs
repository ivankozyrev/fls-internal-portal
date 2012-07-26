using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace FLS.Sharepoint.TwitterSearchWebPart.TwitterSearchWebPart
{
    [ToolboxItemAttribute(false)]
    public class TwitterSearchWebPart : WebPart
    {
        private string query;
        private readonly string defaultProfileImage = "http://static.twitter.com/images/default_profile_normal.png";
        private readonly string sinceIdPrefix = "SinceId::";
        private readonly string urlTemplate = "http://search.twitter.com/search.atom?q={0}&since_id={1}";
        private static XNamespace atomNS = "http://www.w3.org/2005/Atom";
        private static Regex twitterName = new Regex(@"@([\w_]+)", RegexOptions.Compiled);
        private static Regex url = new Regex(@"(https?://([\w-]+\.)+[\w-]+([^ ]*))", RegexOptions.Compiled);

        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("Twitter Search")]
        [WebDisplayName("Display Profile Image")]
        [WebDescription("Display Profile Image")]
        public bool DisplayImage { get; set; }

        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("Twitter Search")]
        [WebDisplayName("Search Text")]
        [WebDescription("Search Text")]
        public string SearchText { get; set; }

        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("Twitter Search")]
        [WebDisplayName("From")]
        [WebDescription("From User")]
        public string From { get; set; }

        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("Twitter Search")]
        [WebDisplayName("To")]
        [WebDescription("To User")]
        public string To { get; set; }

        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("Twitter Search")]
        [WebDisplayName("About")]
        [WebDescription("About User")]
        public string About { get; set; }

        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("Twitter Search")]
        [WebDisplayName("Hashtag")]
        [WebDescription("Hashtag")]
        public string HashTag { get; set; }

        [Personalizable(PersonalizationScope.User)]
        [WebBrowsable(true)]
        [Category("Twitter Search")]
        [WebDisplayName("QueryString Variable")]
        [WebDescription("QueryString Variable")]
        public string QueryStringVariable { get; set; }

        public List<Tweet> CachedTweets
        {
            get
            {
                if (HttpContext.Current.Cache[query] != null)
                {
                    return (List<Tweet>)HttpContext.Current.Cache[query];
                }
                return new List<Tweet>();
            }

            set
            {
                if (HttpContext.Current.Cache[query] == null)
                {
                    HttpContext.Current.Cache.Add(query, value, null, DateTime.Now.AddMinutes(5),
                        Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
                else
                {
                    HttpContext.Current.Cache[query] = value;
                }
            }
        }

        public string CachedSinceId
        {
            get
            {
                if (HttpContext.Current.Cache[sinceIdPrefix + query] != null)
                {
                    return (string)HttpContext.Current.Cache[sinceIdPrefix + query];
                }

                return null;
            }

            set
            {
                if (HttpContext.Current.Cache[this.sinceIdPrefix + this.query] == null)
                {
                    HttpContext.Current.Cache.Add(
                        this.sinceIdPrefix + this.query,
                        value,
                        null,
                        DateTime.Now.AddMinutes(5),
                        Cache.NoSlidingExpiration,
                        CacheItemPriority.Normal,
                        null);
                }
                else
                {
                    HttpContext.Current.Cache[this.sinceIdPrefix + this.query] = value;
                }

            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            try
            {
                this.AddStylesheet();
                BuildQuery();
                DisplayResults(GetTweets());
            }
            catch (Exception ex)
            {
                this.Controls.Add(new LiteralControl(ex.Message + "<br />" +
                    ex.StackTrace + "<br />" + ex.Source));
            }
        }

        private void AddStylesheet()
        {
            var cssLink = new HtmlLink();
            cssLink.Href = "~/_layouts/FLS.Sharepoint/css/twitter.css";
            cssLink.Attributes.Add("rel", "Stylesheet");
            cssLink.Attributes.Add("type", "text/css");
            cssLink.Attributes.Add("media", "all");
            Page.Header.Controls.Add(cssLink);
        }

        private void BuildQuery()
        {
            if (!String.IsNullOrEmpty(QueryStringVariable) &&
                !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[QueryStringVariable]))
            {
                query = HttpContext.Current.Request.QueryString[QueryStringVariable];
            }
            else
            {
                var patterns = new List<string>();
                if (!String.IsNullOrEmpty(SearchText))
                    patterns.Add(SearchText);

                if (!String.IsNullOrEmpty(From))
                    patterns.Add("from:" + From);

                if (!String.IsNullOrEmpty(To))
                    patterns.Add("to:" + To);

                if (!String.IsNullOrEmpty(About))
                    patterns.Add("@" + About);

                if (!String.IsNullOrEmpty(HashTag))
                    patterns.Add("#" + HashTag);

                query = String.Join("+", patterns.ToArray());
                query = HttpUtility.UrlEncode(query);
            }
        }

        private List<Tweet> GetTweets()
        {
            var results = new List<Tweet>();

            if (String.IsNullOrEmpty(query))
                return results;

            var oldResults = CachedTweets;

            var xDoc = XDocument.Load(string.Format(urlTemplate, query, CachedSinceId));

            var newResults =
            (from tweet in xDoc.Descendants(atomNS + "entry")
             select new Tweet
                 {
                     Title = (string)tweet.Element(atomNS + "title"),
                     Published =
                        DateTime.Parse((string)tweet.Element(atomNS + "published")),
                     Id = (string)tweet.Element(atomNS + "id"),
                     Link = tweet.Elements(atomNS + "link")
                        .Where(link => (string)link.Attribute("rel") == "alternate")
                        .Select(link => (string)link.Attribute("href"))
                        .First(),
                     ProfileImage = tweet.Elements(atomNS + "link")
                        .Where(link => (string)link.Attribute("rel") == "image")
                        .Select(link => (string)link.Attribute("href"))
                        .DefaultIfEmpty(defaultProfileImage).First(),
                     Author = (from author in tweet.Descendants(atomNS + "author")
                               select new Author
                               {
                                   Name = (string)author.Element(atomNS + "name"),
                                   Uri = (string)author.Element(atomNS + "uri"),
                               }).First()
                 }).ToList();

            results = oldResults.Concat(newResults).OrderByDescending(tweet => tweet.Id).ToList();

            if (results.Count > 0)
            {
                var idParts = results[0].Id.Split(':');
                CachedSinceId = idParts[idParts.Length - 1];
                CachedTweets = results;
            }

            return results;
        }

        private void DisplayResults(List<Tweet> results)
        {
            var content = new Literal();
            var txt = new StringBuilder();

            var panel = new Panel();
            panel.CssClass = "tweet-searchheader";

            if (!String.IsNullOrEmpty(query))
            {
                txt.Append("<div class='tweet-search'><strong>Search Terms:</strong> " +
                    HttpUtility.HtmlEncode(HttpUtility.UrlDecode(query)) + "</div>");
            }

            if (results.Count == 0)
            {
                txt.Append("<div class=\"tweet-emptysearch\">The search returned no results.</div>");
            }

            content.Text = txt.ToString();
            panel.Controls.Add(content);
            this.Controls.Add(panel);

            if (results.Count == 0)
            {
                return;
            }

            txt = new StringBuilder();
            txt.Append("<ol class=\"tweet-statuses\">");

            foreach (var result in results)
            {
                var authorName = result.Author.Name.Replace("'", "");
                var authorUri = result.Author.Uri;
                var publishDate = result.Published.ToString("G");
                var link = result.Link;
                var title = AddHyperlinks(result.Title);

                txt.Append("<li class=\"tweet-status\">");

                if (DisplayImage)
                {
                    string profileImage = result.ProfileImage;
                    txt.AppendFormat("<span class=\"tweet-thumb\"><a class=\"tweet-url\" href=\"{0}\" " +
                        "target=\"_blank\"><img class=\"tweet-photo\" src=\"{1}\" alt=\"{2}\" /></a></span>",
                        authorUri, profileImage, authorName);
                }

                var bodyClass = (DisplayImage) ? "tweet-statusbody" : "tweet-statusbodyplain";
                txt.AppendFormat("<span class=\"{0}\"><strong><a class=\"screenname\" href=\"{1}\" target=\"_blank\">{2}</a></strong> ",
                    bodyClass, authorUri, authorName);

                txt.AppendFormat("<span class=\"tweet-entrycontent\">{0}</span> ", title);

                txt.AppendFormat("<span class=\"tweet-entrymeta\"><a href=\"{0}\" class=\"tweet-entrydate\" target=\"_blank\">Posted at {1}</a></span></span>",
                    link, publishDate);

                txt.Append("<div class=\"clear\"></div>");

                txt.Append("</li>");
            }

            txt.Append("</ol>");

            panel = new Panel { CssClass = "tweet-container" };
            content = new Literal { Text = txt.ToString() };
            panel.Controls.Add(content);
            this.Controls.Add(panel);

        }

        private static string AddHyperlinks(string text)
        {
            text = url.Replace(text, "<a href=\"$1\" target=\"_blank\">$1</a>");
            text = twitterName.Replace(text, "<a href=\"http://www.twitter.com/$1\" target=\"_blank\">@$1</a>");
            return text;
        }
    }
}
