using System;
using System.Linq;
using System.Runtime.InteropServices;
using FLS.SharePoint.Infrastructure;
using FLS.SharePoint.Utils;
using FLS.SharePoint.Utils.ConfigurationEntities;
using Microsoft.BusinessData.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData.Infrastructure;
using Microsoft.SharePoint.BusinessData.SharedService;

namespace FLS.Sharepoint.FileSearchConnector.Features.OkatoSearchConfigFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("71cdf3ee-087b-4852-bae5-3d0ce2bb05db")]
    public class OkatoSearchConfigFeatureEventReceiver : SPFeatureReceiver
    {
        private static readonly IServiceLocator ServiceLocator = SharePointServiceLocator.GetCurrent();

        private static readonly IConfigPropertiesParser ConfigPropertiesParser = ServiceLocator.GetInstance<IConfigPropertiesParser>();

        private static readonly ILogger Log = ServiceLocator.GetInstance<ILogger>();

        private string _profilePageHostUrl;

        private SitesConfiguration SitesConfiguration;

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            Log.Debug("start activating 'Config OKATO search' feature");

            SetBdcServiceAdministrator(properties);
            CreateBcsEntitiesProfileSite(properties);
            SetOkatoEntityProfilePage(properties);
        }

        private static string GetProfilePageHostUrl(SPFeatureReceiverProperties properties)
        {
            return string.Format("{0}/sites/entityprofilers", GetRootSiteUrl(properties));
        }

        private static string GetRootSiteUrl(SPFeatureReceiverProperties properties)
        {
            return ((SPWebApplication)properties.Feature.Parent).Sites[0].RootWeb.Url;
        }

        private static SPSiteCollection GetSiteCollection(SPFeatureReceiverProperties properties)
        {
            return ((SPWebApplication)properties.Feature.Parent).Sites;
        }

        private static SitesConfiguration GetSitesConfiguration(SPFeatureReceiverProperties properties, IConfigPropertiesParser parser)
        {
            return parser.ParseSitesConfiguration(properties.Feature.Properties["SitesConfiguration"].Value);
        }

        private static bool IsSiteExistsInCollection(SPSite sites, string siteName)
        {
            return sites.AllWebs.Names.Contains(siteName);
        }

        private static bool IsSiteCollectionExists(SPWebApplication application, string siteCollectionRelativeUrl)
        {
            var applicationUrl = string.Empty;
            foreach (var alternateUrl in application.AlternateUrls.Where(alternateUrl => alternateUrl.UrlZone == SPUrlZone.Default))
            {
                applicationUrl = alternateUrl.Uri.AbsoluteUri;
            }

            return !string.IsNullOrEmpty(applicationUrl) && SPSite.Exists(new Uri(applicationUrl + siteCollectionRelativeUrl));
        }

        private static void SetBdcServiceAdministrator(SPFeatureReceiverProperties properties)
        {
            using (var root = new SPSite(GetRootSiteUrl(properties)))
            {
                var service = SPFarm.Local.Services.GetValue<BdcService>();
                var catalog = service.GetAdministrationMetadataCatalog(SPServiceContext.GetContext(root));
                var accessControlList = catalog.GetAccessControlList();
                var accessControlEntry = new IndividualAccessControlEntry(@"i:0#.w|NT AUTHORITY\Network service", BdcRights.Execute | BdcRights.SetPermissions);
                accessControlList.Add(accessControlEntry);
                catalog.SetAccessControlList(accessControlList);
            }
        }

        private static void CreateBcsEntitiesProfileSite(SPFeatureReceiverProperties properties)
        {
            var configuration = GetSitesConfiguration(properties, ConfigPropertiesParser);
            using (var root = new SPSite(GetRootSiteUrl(properties)))
            {
                using (var rootWeb = root.OpenWeb())
                {
                    try
                    {
                        rootWeb.AllowUnsafeUpdates = true;
                        var application = properties.Feature.Parent as SPWebApplication;
                        SPSite newSiteCollection;
                        if (!IsSiteCollectionExists(application, configuration.SiteCollectionName))
                        {
                            newSiteCollection = GetSiteCollection(properties).Add(
                                configuration.SiteCollectionName,
                                "BDC entity profilers",
                                string.Empty,
                                (uint)rootWeb.Locale.LCID,
                                "STS#1",
                                rootWeb.CurrentUser.LoginName,
                                rootWeb.CurrentUser.Name,
                                rootWeb.CurrentUser.Email);
                        }
                        else
                        {
                            newSiteCollection = new SPSite(application.Sites[0].Url + "/" + configuration.SiteCollectionName);
                        }

                        foreach (var site in configuration.Sites)
                        {
                            if (!IsSiteExistsInCollection(newSiteCollection, site.Name))
                            {
                                var newWeb = newSiteCollection.AllWebs.Add(
                                    site.Name,
                                    site.Title,
                                    string.Empty,
                                    (uint)rootWeb.Locale.LCID,
                                    site.WebTemplate,
                                    false,
                                    false);
                                newWeb.Update();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        throw;
                    }
                    finally
                    {
                        rootWeb.AllowUnsafeUpdates = false;
                    }
                }
            }
        }

        private static void SetOkatoEntityProfilePage(SPFeatureReceiverProperties properties)
        {
            using (var root = new SPSite(GetRootSiteUrl(properties)))
            {
                var hostUrl = GetProfilePageHostUrl(properties);
                var service = SPFarm.Local.Services.GetValue<BdcService>();
                var catalog = service.GetAdministrationMetadataCatalog(SPServiceContext.GetContext(root));
                var property =
                    catalog.Properties.FirstOrDefault(c => c.Name.Equals("Profile_HostURL", StringComparison.Ordinal));
                if (property == null)
                {
                    catalog.Properties.Add("Profile_HostURL", hostUrl);
                }
                else
                {
                    catalog.Properties.Remove("Profile_HostURL");
                    catalog.Properties.Add("Profile_HostURL", hostUrl);
                }
            }
        }
    }
}
