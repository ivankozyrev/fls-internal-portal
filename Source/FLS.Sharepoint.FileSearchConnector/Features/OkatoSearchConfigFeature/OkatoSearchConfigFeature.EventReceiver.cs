using System;
using System.Linq;
using System.Runtime.InteropServices;
using FLS.SharePoint.Infrastructure;
using FLS.SharePoint.Utils;
using FLS.SharePoint.Utils.ConfigurationEntities;
using Microsoft.BusinessData.Infrastructure;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData.Infrastructure;
using Microsoft.SharePoint.BusinessData.SharedService;
using Microsoft.SharePoint.Portal.WebControls;

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
        private const string SitesCollectionName = @"sites/entityprofilers";

        private static readonly IServiceLocator ServiceLocator = SharePointServiceLocator.GetCurrent();

        private static readonly IConfigPropertiesParser ConfigPropertiesParser = ServiceLocator.GetInstance<IConfigPropertiesParser>();

        private static readonly ILogger Log = ServiceLocator.GetInstance<ILogger>();

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            Log.Debug("start activating 'Config OKATO search' feature");
            using (var root = new SPSite(GetRootSiteUrl(properties)))
            {
                SetBdcServiceAdministrator(root);
                CreateBdcEntitiesProfileSite(properties, root);
                SetOkatoEntityProfilePage(GetProfilePageHostUrl(properties), root);
                SetOkatoEntityPermissions(root);
                CreateBdcContentSourceForSearchService();
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            base.FeatureDeactivating(properties);
            using (var root = new SPSite(GetRootSiteUrl(properties)))
            {
                DeleteBdcContentSourceForSearchService();
                DeleteOkatoModel(root);
            }
        }

        #region private stuff

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
        
        private static bool IsSiteCollectionExists(SPWebApplication application, string siteCollectionRelativeUrl)
        {
            var applicationUrl = string.Empty;
            foreach (var alternateUrl in application.AlternateUrls.Where(alternateUrl => alternateUrl.UrlZone == SPUrlZone.Default))
            {
                applicationUrl = alternateUrl.Uri.AbsoluteUri;
            }

            return !string.IsNullOrEmpty(applicationUrl) && SPSite.Exists(new Uri(applicationUrl + siteCollectionRelativeUrl));
        }

        private static void SetBdcServiceAdministrator(SPSite root)
        {
            var service = SPFarm.Local.Services.GetValue<BdcService>();
            var catalog = service.GetAdministrationMetadataCatalog(SPServiceContext.GetContext(root));
            var accessControlList = catalog.GetAccessControlList();
            var accessControlEntry = new IndividualAccessControlEntry(@"i:0#.w|NT AUTHORITY\Network service", BdcRights.Execute | BdcRights.SetPermissions);
            accessControlList.Add(accessControlEntry);
            catalog.SetAccessControlList(accessControlList);
        }

        private static void CreateBdcEntitiesProfileSite(SPFeatureReceiverProperties properties, SPSite root)
        {
            var configuration = GetSitesConfiguration(properties, ConfigPropertiesParser);
            try
            {
                var application = (SPWebApplication)properties.Feature.Parent;
                if (IsSiteCollectionExists(application, SitesCollectionName))
                {
                    return;
                }
                
                GetSiteCollection(properties).Add(
                    SitesCollectionName,
                    "BDC entity profilers",
                    string.Empty,
                    (uint)root.RootWeb.Locale.LCID,
                    "STS#1",
                    configuration.SitesOwner,
                    configuration.SitesOwner,
                    string.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        private static void SetOkatoEntityProfilePage(string hostUrl, SPSite root)
        {
            var service = SPFarm.Local.Services.GetValue<BdcService>();
            var serviceContext = SPServiceContext.GetContext(root);
            var catalog = service.GetAdministrationMetadataCatalog(serviceContext);
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

            try
            {
                new ProfileProvisioner(serviceContext).CreateProfilePage("FLS.Sharepoint.FileSearchConnector.OkatoModel", "OkatoEntity");
            }
            catch (ProfileProvisionException ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        private static void SetOkatoEntityPermissions(SPSite root)
        {
            var service = SPFarm.Local.Services.GetValue<BdcService>();
            var serviceContext = SPServiceContext.GetContext(root);
            var catalog = service.GetAdministrationMetadataCatalog(serviceContext);
            var entity = catalog.GetEntity("FLS.Sharepoint.FileSearchConnector.OkatoModel", "OkatoEntity");
            var model = catalog.GetModel("OkatoModel");
            var lobSystem = catalog.GetLobSystem("OkatoModel");
            var entityAccessControlList = entity.GetAccessControlList();
            var modelAccessControlList = model.GetAccessControlList();
            var lobSystemAccessControlList = lobSystem.GetAccessControlList();

            var accessControlEntry = new IndividualAccessControlEntry(
                @"i:0#.w|NT AUTHORITY\Network service", 
                BdcRights.Edit | BdcRights.Execute | BdcRights.SelectableInClients | BdcRights.SetPermissions);
            entityAccessControlList.Add(accessControlEntry);
            modelAccessControlList.Add(accessControlEntry);
            lobSystemAccessControlList.Add(accessControlEntry);
            entity.SetAccessControlList(entityAccessControlList);
            model.SetAccessControlList(entityAccessControlList);
            lobSystem.SetAccessControlList(entityAccessControlList);
        }

        private static void CreateBdcContentSourceForSearchService()
        {
            var application = SPFarm
                .Local
                .Services
                .GetValue<SearchQueryAndSiteSettingsService>()
                .Applications
                .GetValue<SearchServiceApplication>("Search service application");
            var content = new Content(application);
            if (content.ContentSources.OfType<BusinessDataContentSource>().SingleOrDefault(c => c.Name == "Okato dictionary") != null)
            {
                return;
            }

            var bdcs = content.ContentSources.Create(typeof(BusinessDataContentSource), "Okato dictionary");
            bdcs.StartAddresses.Add(BusinessDataContentSource.ConstructStartAddress("default", Guid.Empty, "OkatoModel", "OkatoModel"));
            bdcs.IncrementalCrawlSchedule = new DailySchedule(application)
                                                {
                                                    BeginDay = DateTime.Now.Day,
                                                    BeginMonth = DateTime.Now.Month,
                                                    BeginYear = DateTime.Now.Year,
                                                    StartHour = DateTime.Now.Hour + 1,
                                                    StartMinute = 0,
                                                    RepeatInterval = 30
                                                };
            bdcs.FullCrawlSchedule = new DailySchedule(application)
                                        {
                                            BeginDay = DateTime.Now.Day,
                                            BeginMonth = DateTime.Now.Month,
                                            BeginYear = DateTime.Now.Year,
                                            StartHour = DateTime.Now.Hour,
                                            StartMinute = DateTime.Now.Minute + 5,
                                            DaysInterval = 1,
                                        };
            bdcs.Update();
        }

        private static void DeleteBdcContentSourceForSearchService()
        {
            var application = SPFarm
                .Local
                .Services
                .GetValue<SearchQueryAndSiteSettingsService>()
                .Applications
                .GetValue<SearchServiceApplication>("Search service application");
            var content = new Content(application);
            if (content.ContentSources.OfType<BusinessDataContentSource>().SingleOrDefault(c => c.Name == "Okato dictionary") == null)
            {
                return;
            }

            content.ContentSources["Okato dictionary"].Delete();
        }

        private static void DeleteOkatoModel(SPSite root)
        {
            var service = SPFarm.Local.Services.GetValue<BdcService>();
            var serviceContext = SPServiceContext.GetContext(root);
            var catalog = service.GetAdministrationMetadataCatalog(serviceContext);
            var entity = catalog.GetEntity("FLS.Sharepoint.FileSearchConnector.OkatoModel", "OkatoEntity");
            var model = catalog.GetModel("OkatoModel");
            var lobSystem = catalog.GetLobSystem("OkatoModel");
            if (entity != null)
            {
                entity.Delete();
            }

            if (model != null)
            {
                model.Delete();
            }

            if (lobSystem != null)
            {
                lobSystem.Delete();
            }
        }
        #endregion
    }
}
