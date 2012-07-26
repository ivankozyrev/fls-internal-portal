using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using FLS.SharePoint.Infrastructure;
using FLS.SharePoint.Utils;
using FLS.SharePoint.Utils.ConfigurationEntities;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FLS.SharePoint.SiteStructure.Features.CreateSitesCollection
{
    [Guid("68f9b364-9421-4b00-8408-6908a439bacd")]
    public class CreateSitesCollectionEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            IServiceLocator serviceLocator =
                SharePointServiceLocator.GetCurrent();
            IConfigPropertiesParser configPropertiesParser =
                serviceLocator.GetInstance<IConfigPropertiesParser>();
            ILogger log = serviceLocator.GetInstance<ILogger>();

            log.Debug("start activating feature");
            var configuration = GetSitesConfiguration(properties,
                                                      configPropertiesParser);
            var rootSite = GetRootSite(properties);
            var siteCollection = GetSiteCollection(properties);
            var rootSiteWeb = GetSiteRootWeb(rootSite);

            foreach (var group in configuration.Groups)
            {
                if (!ContainsGroup(rootSiteWeb.SiteGroups, group.Name))
                {
                    var owner = rootSiteWeb.EnsureUser(configuration.SiteOwner);
                    rootSiteWeb.SiteGroups.Add(group.Name, owner, owner,
                                           group.Description);
                }

                var newGroup = rootSiteWeb.SiteGroups[group.Name];
                foreach (var user in group.Users)
                {
                    newGroup.AddUser(user.Login, string.Empty, user.Login,
                                     string.Empty);
                }
            }

            foreach (var site in configuration.Sites)
            {
                if (SiteCollectionExists(siteCollection, site.Name))
                {
                    siteCollection.Delete(site.Name);
                }

                var newSiteCollection = siteCollection.Add(
                    site.Name,
                    site.Title,
                    site.Description,
                    (uint)GetSiteRootWeb(rootSite).Locale.LCID,
                    site.WebTemplate,
                    configuration.SiteOwner,
                    configuration.SiteOwner,
                    string.Empty);

                foreach (var permission in site.Permissions)
                {
                    var roleAssignment =
                        new SPRoleAssignment(
                            rootSiteWeb.SiteGroups[permission.GroupName]);
                    roleAssignment.RoleDefinitionBindings.Add(
                        rootSiteWeb.RoleDefinitions.GetById(permission.PermissionLevelId));
                    GetSiteRootWeb(newSiteCollection).RoleAssignments.Add(roleAssignment);
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            Debugger.Break();
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            IConfigPropertiesParser configPropertiesParser = serviceLocator.GetInstance<IConfigPropertiesParser>();
            
            var configuration = GetSitesConfiguration(properties, configPropertiesParser);
            var sites = GetSiteCollection(properties);
            var rootSite = GetRootSite(properties);
            foreach (var collection in configuration.Sites)
            {
                if (SiteCollectionExists(sites, collection.Name))
                {
                    var site = sites[collection.Name];
                    foreach (var permission in collection.Permissions)
                    {
                        GetSiteRootWeb(site).RoleAssignments.Remove(GetSiteRootWeb(rootSite).SiteGroups[permission.GroupName]);
                    }

                    sites.Delete(collection.Name);
                }
            }

            foreach (var group in configuration.Groups)
            {
                GetSiteRootWeb(rootSite).SiteGroups.Remove(group.Name);
            }
        }

        private static SitesConfiguration GetSitesConfiguration(SPFeatureReceiverProperties properties, IConfigPropertiesParser parser)
        {
            return parser.ParseSitesConfiguration(properties.Feature.Properties["SitesConfiguration"].Value);
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

        private static SPSite GetRootSite(SPFeatureReceiverProperties properties)
        {
            return ((SPWebApplication) properties.Feature.Parent).Sites[0];
        }

        private static SPSiteCollection GetSiteCollection(SPFeatureReceiverProperties properties)
        {
            return ((SPWebApplication)properties.Feature.Parent).Sites;
        }

        private static SPWeb GetSiteRootWeb(SPSite rootSite)
        {
            return rootSite.RootWeb;
        }

        private static bool SiteCollectionExists(SPSiteCollection sites, string siteName)
        {
            return !string.IsNullOrEmpty(sites.Names.FirstOrDefault(site => site == siteName));
        }
    }
}