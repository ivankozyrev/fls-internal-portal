using System.IO;
using System.Runtime.InteropServices;
using FLS.SharePoint.Infrastructure;
using FLS.SharePoint.Utils;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;

namespace FLS.SharePoint.SiteStructure.Features.CreateSitesCollection
{
    [Guid("68f9b364-9421-4b00-8408-6908a439bacd")]
    public class CreateSitesCollectionEventReceiver : SPFeatureReceiver
    {
        private readonly IConfigPropertiesParser configPropertiesParser;
        private readonly IServiceLocator serviceLocator;
        private readonly ILogger log;
        
        public CreateSitesCollectionEventReceiver()
        {
            serviceLocator = SharePointServiceLocator.GetCurrent();
            configPropertiesParser = serviceLocator.GetInstance<IConfigPropertiesParser>();
            log = serviceLocator.GetInstance<ILogger>();
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            log.Debug("Starting activation of a feature");
            var webs = GetAvailableWebs(properties);
            var siteNames = GetSiteNames(properties);
            var siteTitles = configPropertiesParser.ToStringArray(properties.Feature.Properties["SiteTitles"].Value);
            var siteDescriptions =
                configPropertiesParser.ToStringArray(properties.Feature.Properties["SiteDescriptions"].Value);
            for (int index = 0; index < siteNames.Length; index++)
            {
                SafelyRemoveSubSite(siteNames[index], webs);
                webs.Add(
                    siteNames[index],
                    siteTitles[index],
                    siteDescriptions[index],
                    (uint)((SPWeb)properties.Feature.Parent).Locale.LCID,
                    properties.Feature.Properties["WebTemplate"].Value,
                    false, 
                    false);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var webs = GetAvailableWebs(properties);
            if (webs == null)
            {
                return;
            }

            foreach (var sitePath in GetSiteNames(properties))
            {
                SafelyRemoveSubSite(sitePath, webs);
            }
        }

        private static SPWebCollection GetAvailableWebs(SPFeatureReceiverProperties properties)
        {
            return ((SPWeb) properties.Feature.Parent).Site.AllWebs;
        }

        private static void SafelyRemoveSubSite(string siteName, SPWebCollection webs)
        {
            try
            {
                webs.Delete(siteName);
            }
            catch (FileNotFoundException)
            {
                // TODO: add logging
            }
            
        }

        private string[] GetSiteNames(SPFeatureReceiverProperties properties)
        {
            return configPropertiesParser.ToStringArray(properties.Feature.Properties["SiteNames"].Value);
        }
    }
}
