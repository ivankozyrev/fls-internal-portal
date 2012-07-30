using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FLS.SharePoint.Infrastructure.ViewObjects;

namespace FLS.SharePoint.Infrastructure
{
    public class XmlFileManager
    {
        public static IEnumerable<UserProfileViewObject> GetUserProfileList(string filePath)
        {
            var resultList = new List<UserProfileViewObject>();
            if (string.IsNullOrEmpty(filePath))
            {
                return resultList;
            }

            var document = new XmlDocument();
            document.Load(filePath);

            var profileXmlNodeList = document.GetElementsByTagName("profile");
            var login = document.GetElementsByTagName("login");
            var firstName = document.GetElementsByTagName("firstname");
            var lastName = document.GetElementsByTagName("lastname");
            var fullName = document.GetElementsByTagName("fullname");
            var email = document.GetElementsByTagName("email");

            resultList.AddRange(profileXmlNodeList.Cast<object>().Select((t, i) => new UserProfileViewObject
                                                                                       {
                                                                                           Login = login.Item(i).InnerText,
                                                                                           FirstName = firstName.Item(i).InnerText,
                                                                                           LastName = lastName.Item(i).InnerText,
                                                                                           FullName = fullName.Item(i).InnerText,
                                                                                           Email = email.Item(i).InnerText
                                                                                       }));

            return resultList;
        }

        public static IEnumerable<UserProfileProperty> GetPropertyList(string filePath)
        {
            var resultList = new List<UserProfileProperty>();
            if (string.IsNullOrEmpty(filePath))
            {
                return resultList;
            }

            var document = new XmlDocument();
            document.Load(filePath);

            var propertyXmlNodeList = document.GetElementsByTagName("Property");
            var name = document.GetElementsByTagName("Name");
            var displayName = document.GetElementsByTagName("DisplayName");
            var type = document.GetElementsByTagName("Type");
            var multiValueFlag = document.GetElementsByTagName("IsMultiValue");
            var termSet = document.GetElementsByTagName("TermSet");
            var termStoreGroup = document.GetElementsByTagName("TermStoreGroup");

            resultList.AddRange(propertyXmlNodeList.Cast<object>().Select((t, i) => new UserProfileProperty
            {
                Name = name.Item(i).InnerText,
                DisplayName = displayName.Item(i).InnerText,
                Type = type.Item(i).InnerText,
                IsMultiValue = Convert.ToBoolean(multiValueFlag.Item(i).InnerText),
                TermSet = termSet.Item(i).InnerText,
                TermStoreGroup = termStoreGroup.Item(i).InnerText
            }));

            return resultList;
        }
    }
}