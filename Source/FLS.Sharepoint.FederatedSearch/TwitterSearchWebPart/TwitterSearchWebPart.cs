using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace FederatedSearch.TwitterSearchWebPart
{
    [ToolboxItemAttribute(false)]
    public class TwitterSearchWebPart : WebPart
    {
        public enum TwitterMode
        {
            Profile,
            Search
        }

        public enum Interval
        {
            Minute10 = 10,
            Minute15 = 15,
            Minute30 = 30,
            Minute45 = 45,
            Minute60 = 60
        }

        [DefaultValue(TwitterMode.Profile)]
        [Description("Select a category from the dropdown list.")]
        [WebDisplayName("Twitter Mode")]
        [WebBrowsable(true)]
        [Personalizable(PersonalizationScope.User)]
        [Category("Twitter Settings")]
        public TwitterMode Mode { get; set; }

        [DefaultValue("@firstlinesoft")]
        [Description("Select a twitter name")]
        [WebDisplayName("Name")]
        [WebBrowsable(true)]
        [Category("Twitter Settings")]
        [Personalizable(PersonalizationScope.User)]
        public String Name { get; set; }

        [Description("Include Tweet replies")]
        [WebDisplayName("Include replies")]
        [WebBrowsable(true)]
        [Category("Twitter Settings")]
        [Personalizable(PersonalizationScope.User)]
        public bool Replies { get; set; }

        [Description("Include Retweet")]
        [WebDisplayName("Include retweet")]
        [WebBrowsable(true)]
        [Category("Twitter Settings")]
        [Personalizable(PersonalizationScope.User)]
        public bool Retweet { get; set; }

        [DefaultValue(10)]
        [Description("Select number of tweet to display")]
        [WebDisplayName("Count")]
        [WebBrowsable(true)]
        [Category("Twitter Settings")]
        [Personalizable(PersonalizationScope.User)]
        public int Count { get; set; }

        [DefaultValue((int)Interval.Minute10)]
        [Description("Time Interval to refresh web part to display the latest tweet")]
        [WebDisplayName("Time Interval")]
        [WebBrowsable(true)]
        [Category("Twitter Settings")]
        [Personalizable(PersonalizationScope.User)]
        public Interval TimeInterval { set; get; }

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/FederatedSearch/TwitterSearchWebPart/TwitterSearchWebPartUserControl.ascx";

        protected override void CreateChildControls()
        {
            var control = Page.LoadControl(_ascxPath) as TwitterSearchWebPartUserControl;
            control.PropertiesWebPart = this;
            Controls.Add(control);
        }
    }
}
