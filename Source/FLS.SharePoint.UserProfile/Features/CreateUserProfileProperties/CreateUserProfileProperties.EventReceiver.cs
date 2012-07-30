using System;
using System.Linq;
using System.Collections.Generic;
using FLS.SharePoint.Infrastructure;
using System.Runtime.InteropServices;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;

namespace FLS.SharePoint.UserProfile.Features.CreateUserProfileProperties
{
    [Guid("739106d4-5581-4056-bd64-f0bfd10fd3fd")]
    public class CreateUserProfilePropertiesEventReceiver : SPFeatureReceiver
    {
        // \Template\Features\{ProjectName}_{FeautureName}\{ElementName}\{FileName}
        private const string filePathTemplate = @"\Template\Features\FLS.SharePoint.UserProfile_CreateUserProfileProperties\UserProfileElements\CustomPropertyList.xml";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            var logger = serviceLocator.GetInstance<ILogger>();

            var filePath = SPUtility.GetGenericSetupPath(filePathTemplate);

            IEnumerable<FLS.SharePoint.Infrastructure.ViewObjects.UserProfileProperty> propertyList = XmlSettingsHelper.GetPropertyList(filePath);

            SPSite site = ((SPWebApplication)properties.Feature.Parent).Sites[0];
            SPServiceContext context = SPServiceContext.GetContext(site);

            var service = new UserProfileHelper(context, logger);

            try
            {
                logger.Debug("start activating 'Create custom properties' for User Profile feature");
            
                service.CreateProperties(propertyList);

                logger.Debug("feature 'Create custom properties' for User Profile was activated");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                service.RemoveProperties(propertyList.Select(pr => pr.Name));
                throw;
            }
        }


        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            var logger = serviceLocator.GetInstance<ILogger>();
            try
            {
                logger.Debug("start deactivating 'Create custom properties' for User Profile feature");

                var filePath = SPUtility.GetGenericSetupPath(filePathTemplate);
                var propertyList = XmlSettingsHelper.GetPropertyList(filePath);

                SPSite site = ((SPWebApplication)properties.Feature.Parent).Sites[0];
                SPServiceContext context = SPServiceContext.GetContext(site);

                var service = new UserProfileHelper(context, logger);
                service.RemoveProperties(propertyList.Select(pr => pr.Name));

                logger.Debug("feature 'Create custom properties' for User Profile was deactivated");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
