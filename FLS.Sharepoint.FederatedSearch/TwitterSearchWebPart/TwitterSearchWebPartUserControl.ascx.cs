using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using FederatedSearch.TwitterModel;

namespace FederatedSearch.TwitterSearchWebPart
{
    public partial class TwitterSearchWebPartUserControl : UserControl
    {
        public TwitterSearchWebPart PropertiesWebPart;

        protected override void OnInit(EventArgs e)
        {
            Load += OnLoad;
            Timer1.Tick += OnTimerTick;
            base.OnInit(e);
        }

        protected void OnLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisplayTweets();
            }
        }

        protected void OnTimerTick(object sender, EventArgs e)
        {
            SetInterval();
            DisplayTweets();
        }

        protected void DisplayTweets()
        {
            try
            {
                if (PropertiesWebPart.Mode == TwitterSearchWebPart.TwitterMode.Profile)
                {
                    var tweets = GetProfile(PropertiesWebPart.Name, PropertiesWebPart.Count, PropertiesWebPart.Retweet, PropertiesWebPart.Replies);
                    var info = tweets.FirstOrDefault().User;
                    ProfileImage.ImageUrl = info.ProfileImageUrl;
                    Title.Text = info.Name;
                    TwitterResults.DataSource = tweets;
                    TwitterResults.DataBind();
                }
                else
                {
                    var Results = GetSearch(PropertiesWebPart.Name, PropertiesWebPart.Count);
                    Title.Text = PropertiesWebPart.Name;
                    TwitterResults.DataSource = Results;
                    TwitterResults.DataBind();
                }
            }
            catch (Exception ex)
            {
                Error.Text = ex.Message;
                Error.Visible = true;
            }
        }

        private static IList<TwitterStatus> GetSearch(string search, int count)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                "http://search.twitter.com/search.atom?q={0}&rpp={1}",
                HttpUtility.UrlEncode(search),
                count);
            var queryUrl = sb.ToString();
            var results = Query(queryUrl);
            var defaultNS = "{http://www.w3.org/2005/Atom}";
            var statuses = new List<TwitterStatus>();
            var entries = results.Descendants(defaultNS + "entry").Select(s => s);

            foreach (var entry in entries)
            {
                var newUser = new TwitterUser
                {
                    Name = entry.Descendants(defaultNS + "name").FirstOrDefault().Value,
                    ProfileImageUrl = entry.Elements(defaultNS + "link")
                      .Where(link => (string)link.Attribute("rel") == "image")
                      .Select(link => (string)link.Attribute("href"))
                      .First()
                };

                var newStatus = new TwitterStatus
                {
                    CreatedAt = DayAgo(DateTime.Parse(entry.Element(defaultNS + "published").Value)),
                    Text = entry.Element(defaultNS + "content").Value,
                    User = newUser
                };

                statuses.Add(newStatus);
            }

            return statuses.ToList();
        }

        private static IList<TwitterStatus> GetProfile(string screenName, int count, bool includeRetweets, bool includeReplies)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                "http://api.twitter.com/1/statuses/user_timeline.xml?screen_name={0}&count={1}",
                HttpUtility.UrlEncode(screenName),
                count);

            if (includeRetweets)
            {
                sb.AppendFormat("&include_rts=1");
            }

            if (!includeReplies)
            {
                sb.AppendFormat("&exclude_replies=1");
            }

            var queryUrl = sb.ToString();
            var results = Query(queryUrl);
            var statuses = from s in results.Descendants("status")
                           select new TwitterStatus
                           {
                               CreatedAt = DayAgo(DateTime.ParseExact(s.Element("created_at").Value, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture)),
                               Text = s.Element("text").Value.ParseUrl().ParseHashtag().ParseUsername(),
                               User = (from u in s.Descendants("user")
                                       select new TwitterUser
                                       {
                                           Id = u.Element("id").Value,
                                           ScreenName = u.Element("screen_name").Value,
                                           Name = u.Element("name").Value,
                                           Description = u.Element("description").Value,
                                           ProfileImageUrl = u.Element("profile_image_url").Value

                                       }).FirstOrDefault()
                           };

            return statuses.ToList();
        }

        private void SetInterval()
        {
            //Check if time has been changed or not
            if (Timer1.Interval != ((int)PropertiesWebPart.TimeInterval) * 60 * 1000)
            {
                Timer1.Interval = ((int)PropertiesWebPart.TimeInterval) * 60 * 1000;
            }
        }

        private static XDocument Query(string url)
        {
            return XDocument.Load(url);
        }

        private static string DayAgo(DateTime date)
        {
            var timeSpan = DateTime.Now - date;
            if (timeSpan.TotalMinutes < 1)
            {
                return "Less than a minute ago";
            }
            
            if (Math.Round(timeSpan.TotalHours) < 2)
            {
                return string.Format("{0} minutes ago", Math.Round(timeSpan.TotalMinutes));
            }

            return Math.Round(timeSpan.TotalDays) < 2 
                ? string.Format("{0} hours ago", Math.Round(timeSpan.TotalHours))
                : string.Format("{0} days ago", Math.Round(timeSpan.TotalDays));
        }
    }
}
