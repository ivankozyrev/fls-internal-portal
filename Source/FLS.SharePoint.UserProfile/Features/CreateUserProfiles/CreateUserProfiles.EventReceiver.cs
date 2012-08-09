using System;
using System.Linq;
using System.Runtime.InteropServices;
using FLS.SharePoint.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;

namespace FLS.SharePoint.UserProfile.Features.CreateUserProfiles
{
    [Guid("ecba645a-1feb-4414-a8f8-71fab18f146a")]
    public class CreateUserProfilesEventReceiver : SPFeatureReceiver
    {
        // \Template\Features\{ProjectName}_{FeautureName}\{ElementName}\{FileName}
        private const string ProfileFilePathTemplate = @"\Template\Features\FLS.SharePoint.UserProfile_CreateUserProfiles\UserProfileElements\UserList.xml";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            var logger = serviceLocator.GetInstance<ILogger>();

            var filePath = SPUtility.GetGenericSetupPath(ProfileFilePathTemplate);

            var userProfileList = XmlSettingsHelper.GetUserProfileList(filePath);

            using (SPSite site = ((SPWebApplication)properties.Feature.Parent).Sites[0])
            {
                SPServiceContext context = SPServiceContext.GetContext(site);

                var service = new UserProfileHelper(context, logger);

                try
                {
                    logger.Debug("start activating 'Create user profiles' feature");

                    service.CreateOrUpdateUserProfiles(userProfileList);

                    logger.Debug("feature 'Create user profiles' was activated");
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    service.RemoveUserProfiles(userProfileList.Select(s => s.Login));
                    throw;
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
            var logger = serviceLocator.GetInstance<ILogger>();
            try
            {
                logger.Debug("start deactivating 'Create user profiles' feature");

                var filePath = SPUtility.GetGenericSetupPath(ProfileFilePathTemplate);
                var userProfileList = XmlSettingsHelper.GetUserProfileList(filePath);

                using (SPSite site = ((SPWebApplication)properties.Feature.Parent).Sites[0])
                {
                    SPServiceContext context = SPServiceContext.GetContext(site);

                    var service = new UserProfileHelper(context, logger);
                    service.RemoveUserProfiles(userProfileList.Select(s => s.Login));

                    logger.Debug("feature 'Create user profiles' was deactivated");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        // Uncomment the method below to handle the event raised after a feature has been installed.

        // public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        // {
        // }

        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        // public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        // {
        // }

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        // public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        // {
        // }
    }
}
