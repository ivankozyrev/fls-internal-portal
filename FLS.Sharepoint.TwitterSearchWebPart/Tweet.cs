namespace FLS.Sharepoint.TwitterSearchWebPart
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Tweet
    {
        public string Id { get; set; }

        public DateTime Published { get; set; }

        public string Link { get; set; }

        public string Title { get; set; }

        public string ProfileImage { get; set; }

        public Author Author { get; set; }
    }
}
