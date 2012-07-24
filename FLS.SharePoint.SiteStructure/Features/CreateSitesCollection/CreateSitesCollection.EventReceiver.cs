using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FLS.SharePoint.SiteStructure.Features.CreateSitesCollection
{
    [Guid("68f9b364-9421-4b00-8408-6908a439bacd")]
    public class CreateSitesCollectionEventReceiver : SPFeatureReceiver
    {
        // TODO: should be configurable.
        private const string ApplicationUrl = "http://eskurikhin/";
        private const int DefaultLanguage = 1033;
        private const string DefaultTitle = "Sharepoint";
        private const string DefaultDescription = "";
        // TODO: template should be changed after it is created
        private const string DefaultWebTemplate = "";
        private const string AdminLogin = @"DOMAIN\eskurikhin";
        private const string AdminName = "Admin";
        private const string AdminEmail = "admin@domain.com";

        private readonly string[] sitesToAdd = new[]
                                                   {
                                                       "sites/dev", "sites/qa", "sites/hr"
                                                   };

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication application = SPWebApplication.Lookup(new Uri(ApplicationUrl));

            foreach (var sitePath in sitesToAdd)
            {
                AddSubSite(sitePath, application);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            foreach (var sitePath in sitesToAdd)
            {
                SafelyRemoveSubSite(sitePath);
            }
        }

        private static void AddSubSite(string relativePath, SPWebApplication application)
        {
            SafelyRemoveSubSite(relativePath);
            application.Sites.Add(
                relativePath,
                DefaultTitle,
                DefaultDescription,
                DefaultLanguage,
                DefaultWebTemplate,
                AdminLogin,
                AdminName,
                AdminEmail);
        }

        private static void SafelyRemoveSubSite(string relativePath)
        {
            UriBuilder builder = new UriBuilder(ApplicationUrl)
                                     {
                                         Path = relativePath
                                     };

            if (SPSite.Exists(builder.Uri))
            {
                using (SPSite site = new SPSite(builder.ToString()))
                {
                    site.Delete();
                }
            }
        }
    }
}
