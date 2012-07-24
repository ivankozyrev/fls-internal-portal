using System.Runtime.InteropServices;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FLS.SharePoint.SiteStructure.Features.CreateSitesCollection
{
    [Guid("68f9b364-9421-4b00-8408-6908a439bacd")]
    public class CreateSitesCollectionEventReceiver : SPFeatureReceiver
    {
        private const string RootSiteLinkKey = "RootSiteLink";
        private const string SiteNamesKey = "SiteNames";
        
        private readonly IConfigPropertiesParser configPropertiesParser;
        
        public CreateSitesCollectionEventReceiver()
        {
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            configPropertiesParser = serviceLocator.GetInstance<IConfigPropertiesParser>();
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebCollection webs = GetAvailableWebs(properties.Feature.Properties[RootSiteLinkKey].Value);
            if (webs == null)
            {
                return;
            }
            
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
                    configPropertiesParser.ToUInt(properties.Feature.Properties["Locale"].Value),
                    properties.Feature.Properties["WebTemplate"].Value,
                    false, 
                    false);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebCollection webs = GetAvailableWebs(properties.Feature.Properties[RootSiteLinkKey].Value);
            if (webs == null)
            {
                return;
            }

            foreach (var sitePath in GetSiteNames(properties))
            {
                SafelyRemoveSubSite(sitePath, webs);
            }
        }

        private SPWebCollection GetAvailableWebs(string rootSiteLink)
        {
            SPWebApplication app = SPWebApplication.Lookup(configPropertiesParser.ToUri(rootSiteLink));
            return app.Sites.Count > 0 
                ? app.Sites[0].AllWebs 
                : null;
        }

        private static void SafelyRemoveSubSite(string siteName, SPWebCollection webs)
        {
            if (webs[siteName].Exists)
            {
                webs.Delete(siteName);
            }
        }

        private string[] GetSiteNames(SPFeatureReceiverProperties properties)
        {
            return configPropertiesParser.ToStringArray(properties.Feature.Properties[SiteNamesKey].Value);
        }
    }
}
