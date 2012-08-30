using System.Collections.Generic;
using System.Linq;

namespace FLS.SharePoint.BdcModel.BdcModel1
{
    /// <summary>
    /// All the methods for retrieving, updating and deleting data are implemented in this class file.
    /// The samples below show the finder and specific finder method for Entity1.
    /// </summary>
    public class ExternalUserService
    {
        private static string  connectionString = "Data Source=.;integrated security=true;Initial Catalog=SharePointExternalDB";
        private static ExternalServiceDataContext DataContext = new ExternalServiceDataContext(connectionString);

        public static ExternalUser ReadItem(int id)
        {
            var user = DataContext.ExternalUsers.First(i => i.UserID == id);
            return user;
        }

        public static IEnumerable<ExternalUser> ReadList()
        {
            var users = DataContext.ExternalUsers.Take(20);
            return users;
        }

        public static ExternalUser Create(ExternalUser newExternalUser)
        {
            DataContext.ExternalUsers.InsertOnSubmit(newExternalUser);
            DataContext.SubmitChanges();
            return newExternalUser;
        }

        public static void Delete(int userID)
        {
            var contact = DataContext.ExternalUsers.First(i => i.UserID == userID);
            DataContext.ExternalUsers.DeleteOnSubmit(contact);
            DataContext.SubmitChanges();
        }

        public static void Update(ExternalUser externalUserEntity, int userId)
        {
            var userToUpdate = DataContext.ExternalUsers.First(i => i.UserID == userId);
            userToUpdate.Name = externalUserEntity.Name;
            userToUpdate.Email = externalUserEntity.Email;
            DataContext.SubmitChanges();
        }
    }
}
