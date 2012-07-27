using System.Collections.Generic;
using System.Linq;
using FLS.SharePoint.Infrastructure.ViewObjects;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint;

namespace FLS.SharePoint.Infrastructure
{
    public class UserProfileService
    {
        private readonly ILogger spLogger;
        private readonly SPServiceContext spContext;

        public UserProfileService(SPServiceContext context, ILogger logger)
        {
            spContext = context;
            spLogger = logger;
        }

        public void CreateOrUpdateUserProfiles(IEnumerable<UserProfileViewObject> collection)
        {
            var profileManager = new UserProfileManager(spContext);

            foreach (var user in collection)
            {
                UserProfile userProfile;
                if(profileManager.UserExists(user.Login))
                {
                    userProfile = profileManager.GetUserProfile(user.Login);
                }
                else
                {
                    userProfile = profileManager.CreateUserProfile(user.Login);
                    spLogger.DebugFormat("User profile for {0} was created.", user.Login);
                }

                userProfile["WorkEmail"].Add(user.Email);
                userProfile["FirstName"].Add(user.FirstName);
                userProfile["LastName"].Add(user.LastName);
                userProfile["PreferredName"].Add(user.FullName);

                userProfile.Commit();
                spLogger.DebugFormat("User profile for {0} was updated", user.Login);
            }
        }

        public void RemoveUserProfiles(IEnumerable<string> accounts)
        {
            var profileManager = new UserProfileManager(spContext);
                
            foreach (var account in accounts.Where(profileManager.UserExists))
            {
                profileManager.RemoveUserProfile(account);
                spLogger.DebugFormat("User Profile for {0} was removed.", account);
            }
        }
    }
}