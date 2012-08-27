using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FLS.SharePoint.BdcModel.BdcModel1
{
    /// <summary>
    /// All the methods for retrieving, updating and deleting data are implemented in this class file.
    /// The samples below show the finder and specific finder method for Entity1.
    /// </summary>
    public class ExternalUserEntityService
    {
        private static SqlConnection GetSqlConnection()
        {
            var connectionString = "Data Source=.;integrated security=true;Initial Catalog=SharePointExternalDB;MultipleActiveResultSets=True"; 
            var connection = new SqlConnection(connectionString); 
            return connection; 
        }

        /// <summary>
        /// This is a sample specific finder method for Entity1.
        /// If you want to delete or rename the method think about changing the xml in the BDC model file as well.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity1</returns>
        public static ExternalUser ReadItem(int id)
        {
            var externalUser = new ExternalUser();
            var sqlConnection =  GetSqlConnection();
            sqlConnection.Open();
            var sqlCommand = new SqlCommand
                                 {
                                     CommandText = string.Format("SELECT [UserID], [Name], [Email] FROM [SharePointExternalDB].[dbo].[ExternalUser] WHERE UserID = '{0}'", id),
                                     Connection = sqlConnection
                                 };
            var sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (sqlDataReader.Read())
            {
                externalUser.UserID = int.Parse(sqlDataReader[0].ToString());
                externalUser.Name = sqlDataReader[1].ToString();
                externalUser.Email = sqlDataReader[2].ToString();
            }

            sqlConnection.Dispose(); 
            return externalUser; 
        }
        /// <summary>
        /// This is a sample finder method for Entity1.
        /// If you want to delete or rename the method think about changing the xml in the BDC model file as well.
        /// </summary>
        /// <returns>IEnumerable of Entities</returns>
        public static IEnumerable<ExternalUser> ReadList()
        {
            var sqlConnection = GetSqlConnection();
            try
            {
                var externalUsers = new List<ExternalUser>();
                sqlConnection.Open();
                var sqlCommand = new SqlCommand
                                     {
                                         Connection = sqlConnection,
                                         CommandText = "select [UserID], [Name], [Email] from [SharePointExternalDB].[dbo].[ExternalUser]"
                                     };
                var sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (sqlDataReader.Read())
                {
                    var externalUser = new ExternalUser
                                            {
                                                UserID = int.Parse(sqlDataReader[0].ToString()),
                                                Name = sqlDataReader[1].ToString(),
                                                Email = sqlDataReader[2].ToString()
                                            };
                    externalUsers.Add(externalUser);
                 }

                return externalUsers;
            } 
            catch (Exception ex) 
            { 
            } 
            finally 
            { 
                sqlConnection.Dispose(); 
            }
            return null;
        }

        public static void Create(ExternalUser newExternalUserEntity)
        {
            var sqlConnection = GetSqlConnection();
            sqlConnection.Open();

            var sqlCommand = new SqlCommand
            {
                Connection = sqlConnection,
                CommandText = string.Format("INSERT INTO [SPExternalDataBase].[dbo].[ExternalUser]([Name],[Email]) VALUES ({0},{1})", newExternalUserEntity.Name, newExternalUserEntity.Email)
            };
            sqlConnection.Dispose(); 
        }

        public static void Delete(int trainingEventID)
        {
            throw new NotImplementedException();
        }

        public static void Update(ExternalUser externalUserEntity)
        {
            throw new NotImplementedException();
        }
    }
}
