using System.Runtime.InteropServices;
using FLS.SharePoint.Utils;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;

namespace FLS.SharePoint.System.Features.ServiceLocator
{
    [Guid("754ec693-13c1-4d7d-957b-59eec52300db")]
    public class ServiceLocationEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var serviceLocator = SharePointServiceLocator.GetCurrent();
            var typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();

            typeMappings.RegisterTypeMapping<IConfigPropertiesParser, ConfigPropertiesParser>();
            typeMappings.RegisterTypeMapping<IFileSystemHelper, FileSystemHelper>();
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var serviceLocator = SharePointServiceLocator.GetCurrent();
            var typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();
            typeMappings.RemoveTypeMapping<IConfigPropertiesParser>(null);
            typeMappings.RemoveTypeMapping<IFileSystemHelper>(null);
        }
    }
}
