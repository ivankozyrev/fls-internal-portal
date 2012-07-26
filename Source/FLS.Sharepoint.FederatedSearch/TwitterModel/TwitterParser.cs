
using System.Text.RegularExpressions;

namespace FederatedSearch.TwitterModel
{
    public static class TwitterParser
    {
        public static string Link(this string s, string url)
        {
            return string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", url, s);
        }

        public static string ParseUrl(this string s)
        {
            return Regex.Replace(s, @"(http(s)?://)?([\w-]+\.)+[\w-]+(/\S\w[\w- ;,./?%&=]\S*)?", Url);
        }

        public static string ParseUsername(this string s)
        {
            return Regex.Replace(s, "(@)((?:[A-Za-z0-9-_]*))", Username);
        }

        public static string ParseHashtag(this string s)
        {
            return Regex.Replace(s, "(#)((?:[A-Za-z0-9-_]*))", Hashtag);
        }

        private static string Hashtag(Match m)
        {
            var x = m.ToString();
            var tag = x.Replace("#", "%23");
            return x.Link("http://search.twitter.com/search?q=" + tag);
        }

        private static string Username(Match m)
        {
            var x = m.ToString();
            var username = x.Replace("@", "");
            return x.Link("http://twitter.com/" + username);
        }

        private static string Url(Match m)
        {
            var x = m.ToString();
            return x.Link(x);
        }
    }
}
