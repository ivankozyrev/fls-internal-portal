using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace FLS.SharePoint.EnterpriseSearchSite.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("5d8dd90f-e683-4920-ba88-e3cc8233280f")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        private const string SearchSiteUrl = "/sites/search";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

            using (var rootSite = (SPSite)properties.Feature.Parent)
            {
                var app = rootSite.WebApplication.Sites;
                
                        app.Add(
                            SearchSiteUrl, 
                            "Search everything", 
                            "Search site description",
                            1049,
                            "SRCHCEN#0",
                            rootSite.Owner.LoginName,
                            rootSite.Owner.Name,
                            rootSite.Owner.Email
                            );
                
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
//            using (var rootSite = new SPSite(SPContext.Current.Site.Url))
//            {
//                var webCollection = rootSite.AllWebs;
//                webCollection.Delete(SearchSiteUrl);
//            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        ////public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        ////{

        ////}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
