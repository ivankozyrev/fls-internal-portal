using System.Runtime.InteropServices;
using FLS.SharePoint.Utils;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;

namespace FLS.SharePoint.System.Features.ServiceLocator
{
    [Guid("754ec693-13c1-4d7d-957b-59eec52300db")]
    public class ServiceLocationEventReceiver : SPFeatureReceiver
    {
        // NOTE: You should avoid using FeatureActivated method here since it may not be 
        // NOTE: running at a high enough permission level to update type mapping.
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            var serviceLocator = SharePointServiceLocator.GetCurrent();
            var typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();

            typeMappings.RegisterTypeMapping<IConfigPropertiesParser, ConfigPropertiesParser>();
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            var serviceLocator = SharePointServiceLocator.GetCurrent();
            var typeMappings = serviceLocator.GetInstance<IServiceLocatorConfig>();
            typeMappings.RemoveTypeMapping<IConfigPropertiesParser>(null);
        }
    }
}
