using System.IO;
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
            // creating new site collection
            var siteCollections = GetSiteCollections(properties);
            if (SiteCollectionExists(siteCollections, configuration.SiteCollectionName))
            {
                siteCollections.Delete(configuration.SiteCollectionName);
            }

            SPSite rootSite = siteCollections.Add(
                configuration.SiteCollectionName,
                configuration.SiteOwner,
                string.Empty);

            // Creating groups
            var rootSiteWebs = GetRootSiteWebs(rootSite);
            var rootSiteWeb = GetRootSiteWeb(rootSite);

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

            // Creating subsites and permissions
            foreach (var site in configuration.Sites)
            {
                SafelyRemoveSubSite(site.Name, rootSiteWebs);
                var newSite = rootSiteWebs.Add(
                    site.Name,
                    site.Title,
                    site.Description,
                    (uint) rootSiteWeb.Locale.LCID,
                    site.WebTemplate,
                    true,
                    false);

                foreach (var permission in site.Permissions)
                {
                    var roleAssignment =
                        new SPRoleAssignment(
                            rootSiteWeb.SiteGroups[permission.GroupName]);
                    roleAssignment.RoleDefinitionBindings.Add(
                        rootSiteWeb.RoleDefinitions.GetById(permission.PermissionLevelId));
                    newSite.RoleAssignments.Add(roleAssignment);
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            IConfigPropertiesParser configPropertiesParser = serviceLocator.GetInstance<IConfigPropertiesParser>();
            
            var configuration = GetSitesConfiguration(properties, configPropertiesParser);
            var siteCollections = GetSiteCollections(properties);

            if (SiteCollectionExists(siteCollections, configuration.SiteCollectionName))
            {
                var rootSite = siteCollections[configuration.SiteCollectionName];
                var rootSiteWebs = GetRootSiteWebs(rootSite);
                var rootWeb = GetRootSiteWeb(rootSite);

                foreach (var site in configuration.Sites)
                {
                    var web = rootSiteWebs[site.Name];

                    foreach (var permission in site.Permissions)
                    {
                        web.RoleAssignments.Remove(rootWeb.SiteGroups[permission.GroupName]);
                    }

                    SafelyRemoveSubSite(site.Name, rootSiteWebs);
                }

                foreach (var group in configuration.Groups)
                {
                    rootWeb.SiteGroups.Remove(group.Name);
                }

                siteCollections.Delete(configuration.SiteCollectionName);
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

        private static SPSiteCollection GetSiteCollections(SPFeatureReceiverProperties properties)
        {
            return ((SPWebApplication) properties.Feature.Parent).Sites;
        }

        private static SPWebCollection GetRootSiteWebs(SPSite rootSite)
        {
            return rootSite.AllWebs;
        }

        private static SPWeb GetRootSiteWeb(SPSite rootSite)
        {
            return rootSite.RootWeb;
        }

        private static void SafelyRemoveSubSite(string siteName, SPWebCollection websToRemoveFrom)
        {
            try
            {
                websToRemoveFrom.Delete(siteName);
            }
            catch (FileNotFoundException)
            {
            }
        }

        private static bool SiteCollectionExists(SPSiteCollection sites, string siteName)
        {
            return !string.IsNullOrEmpty(sites.Names.FirstOrDefault(site => site == siteName));
        }
    }
}