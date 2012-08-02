using System;
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
        private readonly IServiceLocator serviceLocator;

        private readonly IConfigPropertiesParser configPropertiesParser;

        private readonly IFileSystemHelper fileSystemHelper;

        private readonly ILogger logger;

        public CreateSitesCollectionEventReceiver()
        {
            serviceLocator = SharePointServiceLocator.GetCurrent();
            configPropertiesParser = serviceLocator.GetInstance<IConfigPropertiesParser>();
            fileSystemHelper = serviceLocator.GetInstance<IFileSystemHelper>();
            logger = serviceLocator.GetInstance<ILogger>();
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            logger.Debug(string.Format("Start feature {0} activation.", properties.Feature.Definition.DisplayName));
            logger.Debug("Reading configuration...");
            var configuration = GetSitesConfiguration(properties,
                                                      configPropertiesParser);
            logger.Debug("Done. Reading configuration.");
            var rootSite = GetRootSite(properties);
            var sites = GetSiteCollections(properties);
            var rootWeb = GetSiteRootWeb(rootSite);

            logger.Debug("Creating groups...");
            foreach (var group in configuration.Groups)
            {
                if (!ContainsGroup(rootWeb.SiteGroups, group.Name))
                {
                    var owner = rootWeb.EnsureUser(configuration.SitesOwner);
                    rootWeb.SiteGroups.Add(group.Name, owner, owner,
                                           group.Description);
                }

                var newGroup = rootWeb.SiteGroups[group.Name];
                foreach (var user in group.Users)
                {
                    newGroup.AddUser(user.Login, string.Empty, user.Login,
                                     string.Empty);
                }
            }

            logger.Debug("Done. Creation of groups");
            foreach (var site in configuration.Sites)
            {
                logger.Debug(string.Format("creating site: {0}", site.Name));
                if (SiteCollectionExists(sites, site.Name))
                {
                    logger.Debug(string.Format("Site {0} already exists. Attempting to delete it.", site.Name));
                    sites.Delete(site.Name);
                    logger.Debug(string.Format("Done. Site {0} deletion.", site.Name));
                }

                logger.Debug(string.Format("Creating new empty site: {0}", site.Name));
                var newSite = sites.Add(
                    site.Name,
                    site.Title,
                    site.Description,
                    GetLocale(rootWeb),
                    null,
                    configuration.SitesOwner,
                    configuration.SitesOwner,
                    string.Empty);
                logger.Debug(string.Format("Done. Empty site {0} creation.", site.Name));
                
                logger.Debug(
                    string.Format(
                        "Loading template: {0} from package: {1} in folder: {2}", 
                        site.WebTemplate.Name, 
                        site.WebTemplate.PackageFileName, 
                        configuration.TemplatesDirectory));
                LoadTemplateFromPackage(
                    newSite, 
                    site.WebTemplate.PackageFileName, 
                    configuration.TemplatesDirectory);
                logger.Debug(string.Format("Done. Loading template: {0}", site.WebTemplate.Name));

                logger.Debug(string.Format(
                    "Applying template: {0} to empty site: {1}", 
                    site.WebTemplate.Name, 
                    site.Name));
                ApplyWebTemplate(GetSiteRootWeb(newSite), site.WebTemplate.Name);
                logger.Debug(string.Format(
                    "Done. Applying template: {0} to site: {1}", 
                    site.WebTemplate.Name, 
                    site.Name));

                logger.Debug(string.Format("Granting permissions for site {0}", site.Name));
                foreach (var permission in site.Permissions)
                {
                    logger.Debug(string.Format(
                        "Granting permissionLevel: {0} to group: {1} on site {2}", 
                        permission.PermissionLevelId, 
                        permission.GroupName, 
                        site.Name));
                    var roleAssignment = new SPRoleAssignment(rootWeb.SiteGroups[permission.GroupName]);
                    roleAssignment.RoleDefinitionBindings.Add(rootWeb.RoleDefinitions.GetById(permission.PermissionLevelId));
                    GetSiteRootWeb(newSite).RoleAssignments.Add(roleAssignment);
                    logger.Debug(string.Format(
                        "Done. Granting permissionLevel: {0} to group: {1} on site {2}", 
                        permission.PermissionLevelId, 
                        permission.GroupName, 
                        site.Name));
                }
                logger.Debug(string.Format("Done. Granting permissions for site {0}", site.Name));
            }

            logger.Debug(string.Format("Done. Feature {0} activation.", properties.Feature.Definition.DisplayName));
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            logger.Debug(string.Format("Start feature {0} deactivation", properties.Feature.Definition.DisplayName));
            logger.Debug("Reading configuration...");
            var configuration = GetSitesConfiguration(properties, configPropertiesParser);
            logger.Debug("Done. Reading configuration");

            var sites = GetSiteCollections(properties);
            var rootSite = GetRootSite(properties);

            logger.Debug("Start deleting sites...");
            foreach (var site in configuration.Sites)
            {
                logger.Debug(string.Format("Processing site: {0}", site.Name));
                if (SiteCollectionExists(sites, site.Name))
                {
                    logger.Debug(string.Format("Site: {0} has been found. Deleting permissions.", site.Name));
                    var siteToDelete = sites[site.Name];
                    foreach (var permission in site.Permissions)
                    {
                        logger.Debug(string.Format(
                            "Deleting permission level: {0} from group: {1} for site: {2}", 
                            permission.PermissionLevelId, 
                            permission.GroupName, 
                            site.Name));
                        GetSiteRootWeb(siteToDelete).RoleAssignments.Remove(GetSiteRootWeb(rootSite).SiteGroups[permission.GroupName]);
                        logger.Debug(string.Format("Done. Deleting permission level: {0} from group: {1} for site: {2}", 
                            permission.PermissionLevelId, 
                            permission.GroupName, 
                            site.Name));
                    }

                    logger.Debug(string.Format("Done. Deleting permissions for site {0}", site.Name));
                    logger.Debug(string.Format("Deleting site: {0} itself", site.Name));
                    sites.Delete(site.Name);
                    logger.Debug(string.Format("Done. Deleting site {0}", site.Name));
                }
                else
                {
                    logger.Debug(string.Format("Can't locate site: {0}. Skipped", site.Name));
                }

                logger.Debug(string.Format("Done. Processing site {0}", site.Name));
            }
            logger.Debug("Done. Deleting sites.");

            logger.Debug("Deleting groups");
            foreach (var group in configuration.Groups)
            {
                logger.Debug(string.Format("Deleteng group: {0}", group.Name));
                GetSiteRootWeb(rootSite).SiteGroups.Remove(group.Name);
                logger.Debug(string.Format("Done. Deleting group: {0}", group.Name));
            }
            logger.Debug("Done. Deleting groups");

            logger.Debug(string.Format("Done. Feature {0} deactivation", properties.Feature.Definition.DisplayName));
        }

        private static SitesConfiguration GetSitesConfiguration(
            SPFeatureReceiverProperties properties, 
            IConfigPropertiesParser parser)
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

        private void ApplyWebTemplate(SPWeb web, string templateName)
        {
            SPWebTemplateCollection templates = web.GetAvailableWebTemplates(GetLocale(web));
            var template = (from SPWebTemplate t in templates
                            where t.Title == templateName
                            select t).FirstOrDefault();

            if (template != null)
            {
                try
                {
                    web.ApplyWebTemplate(template);
                }
                catch (SPException exception)
                {
                    logger.Debug(exception.NativeErrorMessage);
                }
            }
        }

        private void LoadTemplateFromPackage(SPSite site, string packageFileName, string packageDirectory)
        {
            SPDocumentLibrary solutions = (SPDocumentLibrary)site.GetCatalog(SPListTemplateType.SolutionCatalog);
            SPFile solutionFile = solutions.RootFolder.Files.Add(
                packageFileName,
                fileSystemHelper.ReadFile(
                    fileSystemHelper.CombinePath(packageDirectory, packageFileName)));

            SPUserSolution solution = site.Solutions.Add(solutionFile.Item.ID);
            Guid solutionId = solution.SolutionId;
            SPFeatureDefinitionCollection siteFeatures = site.FeatureDefinitions;
            var features = from SPFeatureDefinition f
                               in siteFeatures
                           where f.SolutionId.Equals(solutionId) && f.Scope == SPFeatureScope.Site
                           select f;
            foreach (SPFeatureDefinition feature in features)
            {
                site.Features.Add(feature.Id, false, SPFeatureDefinitionScope.Site);
            }
        }

        private static SPSiteCollection GetSiteCollections(SPFeatureReceiverProperties properties)
        {
            return ((SPWebApplication)properties.Feature.Parent).Sites;
        }

        private static SPWeb GetSiteRootWeb(SPSite rootSite)
        {
            return rootSite.RootWeb;
        }

        private static uint GetLocale(SPWeb web)
        {
            return (uint)web.Locale.LCID;
        }

        private static bool SiteCollectionExists(SPSiteCollection sites, string siteName)
        {
            return !string.IsNullOrEmpty(sites.Names.FirstOrDefault(site => site == siteName));
        }
    }
}