using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.Search.Query;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;
using System.Linq;

namespace FederatedSearch.Features.FederatedSearchFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("e415d41d-e7e2-400a-a3cf-7275a323e461")]
    public class FederatedSearchFeatureEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            if (!(properties.Feature.Parent is SPWebService)) return;

            var svc = properties.Feature.Parent as SPWebService;
            var serviceApp = svc.WebApplications.OfType<SearchServiceApplication>().FirstOrDefault();
            
            if (serviceApp == null) return;
            
            var osdxFileList = new[] { "twitter", "flickr", "wikipedia", "youtube", "googlenews", "msdn" };
            foreach (var item in osdxFileList)
            {
                serviceApp.AddNewLocationConfiguration(GetFederatedLocation(string.Format("~layouts/fls.sharepoint.federatedsearch/osdx/{0}.osdx", item)));    
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}

        private LocationConfiguration GetFederatedLocation(string osdxPath)
        {
            var newLocation = new LocationConfiguration();
            using (var fs = new FileStream(osdxPath, FileMode.Open))
            {
                newLocation.Import(fs);
                newLocation.Update();
            }

            return newLocation;
        }

    }
}
