using System.Runtime.InteropServices;
using FLS.SharePoint.Utils;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FLS.SharePoint.System.Features.ServiceLocator
{
    [Guid("754ec693-13c1-4d7d-957b-59eec52300db")]
    public class ServiceLocationEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var serviceLocator = SharePointServiceLocator.GetCurrent();
            var typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();

            // TODO: dirty approach. Should use farm scope and full trust proxy to access mappings from sandboxed code
            var applications = SPWebService.ContentService.WebApplications;
            foreach (var application in applications)
            {
                foreach (SPSite site in application.Sites)
                {
                    RegisterMapping(typeMappings, site);
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var serviceLocator = SharePointServiceLocator.GetCurrent();
            var typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();

            // TODO: dirty approach. Should use farm scope and full trust proxy to access mappings from sandboxed code
            var applications = SPWebService.ContentService.WebApplications;
            foreach (var application in applications)
            {
                foreach (SPSite site in application.Sites)
                {
                    UnregisterMapping(typeMappings, site);
                }
            }
        }

        private static void RegisterMapping(IServiceLocatorConfig typeMappings, SPSite site)
        {
            typeMappings.Site = site;
            typeMappings.RegisterTypeMapping<IConfigPropertiesParser, ConfigPropertiesParser>();
            typeMappings.RegisterTypeMapping<IFileSystemHelper, FileSystemHelper>();
        }

        private static void UnregisterMapping(IServiceLocatorConfig typeMappings, SPSite site)
        {
            typeMappings.Site = site;
            typeMappings.RemoveTypeMapping<IConfigPropertiesParser>(null);
            typeMappings.RemoveTypeMapping<IFileSystemHelper>(null);
        }
    }
}
