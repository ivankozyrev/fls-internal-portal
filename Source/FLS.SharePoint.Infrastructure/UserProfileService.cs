﻿using System.Collections;
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
                if (profileManager.UserExists(user.Login))
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

        public void CreateProperties(IEnumerable<UserProfileProperty> properties)
        {
            var configManager = new UserProfileConfigManager(spContext);
            var profilepropertyManager = configManager.ProfilePropertyManager;
            var corePropertyManager = profilepropertyManager.GetCoreProperties();

            var propertyTypeCollection = configManager.GetPropertyDataTypes();
            foreach (var property in properties)
            {
                var existProperty = corePropertyManager.GetPropertyByName(property.Name);
                if (existProperty == null)
                {
                    var propertyType = GetPropertyType(propertyTypeCollection, property.Type);
                    if (propertyType == null)
                    {
                        spLogger.DebugFormat(
                            "Property {0} was not created, because type {1} doesn't exist",
                            property.Name,
                            property.Type);
                        continue;
                    }

                    var coreProperty = corePropertyManager.Create(false);
                    coreProperty.Name = property.Name;
                    coreProperty.DisplayName = property.DisplayName;
                    coreProperty.Type = propertyType.Name;

                    coreProperty.IsMultivalued = property.IsMultiValue;
                    if (property.IsMultiValue)
                    {
                        coreProperty.Separator = MultiValueSeparator.Semicolon;
                    }

                    corePropertyManager.Add(coreProperty);

                    var profileTypePropertyManager =
                        profilepropertyManager.GetProfileTypeProperties(ProfileType.User);
                    var profileTypeProperty = profileTypePropertyManager.Create(coreProperty);
                    profileTypeProperty.IsVisibleOnEditor = true;
                    profileTypeProperty.IsVisibleOnViewer = true;

                    profileTypePropertyManager.Add(profileTypeProperty);

                    var profileSubTypeManager = ProfileSubtypeManager.Get(spContext);
                    var profileSubType =
                        profileSubTypeManager.GetProfileSubtype(
                            ProfileSubtypeManager.GetDefaultProfileName(ProfileType.User));

                    var profileSubTypeProperties = profileSubType.Properties;

                    var profileSubTypeProperty = profileSubTypeProperties.Create(profileTypeProperty);

                    profileSubTypeProperty.PrivacyPolicy = PrivacyPolicy.OptIn;
                    profileSubTypeProperty.DefaultPrivacy = Privacy.Public;
                    profileSubTypeProperty.IsUserEditable = true;

                    profileSubTypeProperties.Add(profileSubTypeProperty);

                    spLogger.DebugFormat("Property {0} was created", property.Name);
                }
                else
                {
                    spLogger.DebugFormat("Property {0} already exists", property.Name);
                }
            }
        }

        public void RemoveProperties(IEnumerable<string> propertyNameCollection)
        {
            var configManager = new UserProfileConfigManager(spContext);
            var profilepropertyManager = configManager.ProfilePropertyManager;
            var corePropertyManager = profilepropertyManager.GetCoreProperties();
            foreach (var propertyName in propertyNameCollection)
            {
                var existProperty = corePropertyManager.GetPropertyByName(propertyName);
                if (existProperty != null)
                {
                    corePropertyManager.RemovePropertyByName(propertyName);
                    spLogger.DebugFormat("Property {0} was removed.", propertyName);
                }
                else
                {
                    spLogger.DebugFormat("Property {0} wasn't removed, because it doesn't exist.", propertyName);
                }
            }
        }

        private static PropertyDataType GetPropertyType(IEnumerable collection, string typeName)
        {
            return collection.Cast<PropertyDataType>().FirstOrDefault(pr => pr.Name == typeName);
        }
    }
}