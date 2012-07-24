using System.Runtime.InteropServices;
using System.Security.Permissions;
using FLS.SharePoint.Utils;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace FLS.SharePoint.System.Features.ServiceLocation
{
    [Guid("754ec693-13c1-4d7d-957b-59eec52300db")]
    public class ServiceLocationEventReceiver : SPFeatureReceiver
    {
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            IServiceLocatorConfig typeMappings =
                                      serviceLocator.GetInstance<IServiceLocatorConfig>();

            typeMappings.RegisterTypeMapping<IConfigPropertiesParser, ConfigPropertiesParser>();
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            IServiceLocatorConfig typeMapping = serviceLocator.GetInstance<IServiceLocatorConfig>();
            typeMapping.RemoveTypeMapping<IConfigPropertiesParser>(null);
        }
    }
}
