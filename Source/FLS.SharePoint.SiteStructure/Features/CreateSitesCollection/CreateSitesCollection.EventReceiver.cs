using System.IO;
using System.Runtime.InteropServices;
using FLS.SharePoint.Infrastructure;
using FLS.SharePoint.Utils;
using FLS.SharePoint.Utils.ConfigurationEntities;
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
            log.Debug("start activating feature");
            var rootWebChildren = GetAvailableWebs(properties);
            var configuration = GetSitesConfiguration(properties);

            var rootWeb = GetFeatureRootWeb(properties);
            foreach (var group in configuration.Groups)
            {
                if (!ContainsGroup(rootWeb.SiteGroups, group.Name))
                {
                    var owner = rootWeb.EnsureUser(configuration.SiteOwner);
                    rootWeb.SiteGroups.Add(group.Name, owner, owner, group.Description);
                }

                var newGroup = rootWeb.SiteGroups[group.Name];
                foreach (var user in group.Users)
                {
                    newGroup.AddUser(user.Login, string.Empty, user.Login, string.Empty);
                }
            }
            
            foreach (var site in configuration.Sites)
            {
                SafelyRemoveSubSite(site.Name, rootWebChildren);
                var newSite = rootWebChildren.Add(
                    site.Name, 
                    site.Title,
                    site.Description,
                    (uint)GetFeatureParent(properties).Locale.LCID,
                    site.WebTemplate,
                    true,
                    false);

                foreach (var permission in site.Permissions)
                {
                    var roleAssignment = new SPRoleAssignment(rootWeb.SiteGroups[permission.GroupName]);
                    roleAssignment.RoleDefinitionBindings.Add(rootWeb.RoleDefinitions[permission.PermissionLevel]);
                    newSite.RoleAssignments.Add(roleAssignment);
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var rootWebChildren = GetAvailableWebs(properties);
            if (rootWebChildren == null)
            {
                return;
            }

            var configuration = GetSitesConfiguration(properties);
            var rootWeb = GetFeatureRootWeb(properties);
            foreach (var site in configuration.Sites)
            {
                var web = rootWebChildren[site.Name];
                
                foreach (var permission in site.Permissions)
                {
                    web.RoleAssignments.Remove(rootWeb.SiteGroups[permission.GroupName]);
                }
                
                SafelyRemoveSubSite(site.Name, rootWebChildren);
            }


            foreach (var group in configuration.Groups)
            {
                rootWeb.SiteGroups.Remove(@group.Name);
            }
        }

        private SitesConfiguration GetSitesConfiguration(SPFeatureReceiverProperties properties)
        {
            return configPropertiesParser.ParseSitesConfiguration(properties.Feature.Properties["SitesConfiguration"].Value);
        }

        private static bool ContainsGroup(SPGroupCollection groupCollection, string groupName)
        {
            try
            {
                var group = groupCollection[groupName];
                return true;
            }
            catch (SPException)
            {
                return false;
            }
        }

        private static SPWebCollection GetAvailableWebs(SPFeatureReceiverProperties properties)
        {
            return GetFeatureParent(properties).Site.AllWebs;
        }

        private static SPWeb GetFeatureParent(SPFeatureReceiverProperties properties)
        {
            return (SPWeb)properties.Feature.Parent;
        }

        private static SPWeb GetFeatureRootWeb(SPFeatureReceiverProperties properties)
        {
            return GetFeatureParent(properties).Site.RootWeb;
        }

        private static void SafelyRemoveSubSite(string siteName, SPWebCollection websToRemoveFrom)
        {
            try
            {
                websToRemoveFrom.Delete(siteName);
            }
            catch (FileNotFoundException)
            {
                // TODO: add logging
            }
            
        }
    }
}
