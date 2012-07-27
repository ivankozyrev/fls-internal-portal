using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FLS.SharePoint.Infrastructure.ViewObjects;

namespace FLS.SharePoint.Infrastructure
{
    public static class XmlFileManager
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
    }
}