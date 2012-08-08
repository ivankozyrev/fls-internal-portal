using System.Linq;
using FLS.Sharepoint.Metro.UI.Utilites;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace FLS.Sharepoint.Metro.UI.Common.Branding.Metro
{
    public class SiteFeatureReceiver : SPFeatureReceiver
    {
        private readonly string[] _searchWebTemplates = new[] { "SRCHCENTERLITE", "SRCHCEN", "SRCHCENTERFAST" };
        private const string MasterPagePath = @"_catalogs/masterpage/";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var site = (SPSite)properties.Feature.Parent;
            var masterPage = properties.Feature.Properties["MasterPage"].Value;
            var searchMasterPage = properties.Feature.Properties["SearchMasterPage"].Value;
            if (string.IsNullOrEmpty(masterPage))
            {
                return;
            }

            var masterPageUrl = SPUrlUtility.CombineUrl(site.ServerRelativeUrl, MasterPagePath + masterPage);
            var searchMasterPageUrl = SPUrlUtility.CombineUrl(site.ServerRelativeUrl, MasterPagePath + searchMasterPage);
            SetMasterPageTemplateAndUpdate(site, masterPageUrl, searchMasterPageUrl);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var site = (SPSite)properties.Feature.Parent;
            var masterPageUrl = SPUrlUtility.CombineUrl(site.ServerRelativeUrl, MasterPagePath + "v4.master");
            var searchMasterPageUrl = SPUrlUtility.CombineUrl(site.ServerRelativeUrl, MasterPagePath + "minimal.master");
            SetMasterPageTemplateAndUpdate(site, masterPageUrl, searchMasterPageUrl);

            SPContext.Current.Site.RootWeb.RemovePropertyAndUpdate("MetroThemeColor");
            SPContext.Current.Site.RootWeb.RemovePropertyAndUpdate("MetroAccordionActivated");
            var lists = site.RootWeb.Lists;
            var metroList = lists["Metro UI Themes"];
            lists.Delete(metroList.ID);
        }

        private void SetMasterPageTemplateAndUpdate(SPSite site, string masterPageUrl, string searchMasterPageUrl)
        {
            foreach (SPWeb web in site.AllWebs)
            {
                web.MasterUrl = masterPageUrl;
                web.CustomMasterUrl = _searchWebTemplates.Contains(web.WebTemplate)
                                          ? searchMasterPageUrl
                                          : masterPageUrl;
                web.Update();
            }
        }
    }
}
