using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FLS.SharePoint.BdcModel.BdcModel1
{
    /// <summary>
    /// All the methods for retrieving, updating and deleting data are implemented in this class file.
    /// The samples below show the finder and specific finder method for Entity1.
    /// </summary>
    public class TrainingEventEntityService
    {
        private static void GetDataModel()
        {
            var connectionString = "Data Source=.;integrated security=true;Initial Catalog=SPExternalDataBase;MultipleActiveResultSets=True";
            var dataModel = new TrainingDataModelDataContext(connectionString);
        }

        private static SqlConnection GetSqlConnection()
        {
            var connectionString = "Data Source=.;integrated security=true;Initial Catalog=SPExternalDataBase;MultipleActiveResultSets=True"; 
            var connection = new SqlConnection (connectionString); 
            return connection; 
        }

        /// <summary>
        /// This is a sample specific finder method for Entity1.
        /// If you want to delete or rename the method think about changing the xml in the BDC model file as well.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity1</returns>
        public static TrainingEvent ReadItem(int id)
        {
            var trainingEvent = new TrainingEvent();
            var sqlConnection =  GetSqlConnection();
            sqlConnection.Open();
            var sqlCommand = new SqlCommand
                                 {
                                     CommandText = "select TrainingEventId, StudentID, TrainingID, EventDate, Status " +
                                                   "from [SPExternalDataBase].[dbo].[TrainingEvent]"
                                                   + "WHERE TrainingEventId =" + id.ToString(),
                                     Connection = sqlConnection
                                 };
            var sqlDataReader = sqlCommand.ExecuteReader (CommandBehavior. CloseConnection);
            if (sqlDataReader.Read ())
            {
                trainingEvent.TrainingEventID = int.Parse(sqlDataReader[0].ToString());
                trainingEvent.StudentID = int.Parse(sqlDataReader[1].ToString());
                trainingEvent.TrainingID = int.Parse(sqlDataReader[2].ToString());
                trainingEvent.EventDate = DateTime.Parse(sqlDataReader[3].ToString());
                trainingEvent.Status = sqlDataReader[4].ToString();
            }
            sqlConnection.Dispose(); 
            return trainingEvent; 
        }
        /// <summary>
        /// This is a sample finder method for Entity1.
        /// If you want to delete or rename the method think about changing the xml in the BDC model file as well.
        /// </summary>
        /// <returns>IEnumerable of Entities</returns>
        public static IEnumerable<TrainingEvent> ReadList()
        {
            var sqlConnection = GetSqlConnection();
            try
            {
                var trainingEvents = new List<TrainingEvent>();
                sqlConnection.Open();
                var sqlCommand = new SqlCommand
                                     {
                                         Connection = sqlConnection,
                                         CommandText = "select TrainingEventId, StudentID, TrainingID, EventDate, Status " +
                                                  "from [SPExternalDataBase].[dbo].[TrainingEvent]"
                                     };
                var sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (sqlDataReader.Read())
                {
                    var trainingEvent = new TrainingEvent
                                            {
                                                TrainingEventID = int.Parse(sqlDataReader[0].ToString()),
                                                StudentID = int.Parse(sqlDataReader[1].ToString()),
                                                TrainingID = int.Parse(sqlDataReader[2].ToString()),
                                                EventDate = DateTime.Parse(sqlDataReader[3].ToString()),
                                                Status = sqlDataReader[4].ToString()
                                            };
                    trainingEvents.Add(trainingEvent);
                 }

                return trainingEvents;
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

        public static TrainingEvent Create(TrainingEvent newTrainingEventEntity)
        {
            var sqlConnection = GetSqlConnection();
            sqlConnection.Open();

            var trainingEvent = new TrainingEvent
                                    {
                                        Title = newTrainingEventEntity.Title,
                                        Description = newTrainingEventEntity.Description,
                                        Status = newTrainingEventEntity.Status,
                                        LoginName = newTrainingEventEntity.LoginName,
                                        TrainingEventID = newTrainingEventEntity.TrainingEventID,
                                        EventType = newTrainingEventEntity.EventType,
                                        EventDate = newTrainingEventEntity.EventDate
                                    };

//
//            sqlConnection.TrainingEvent.InsertOnSubmit(trainingEvent);
//            sqlConnection.SubmitChanges();
            return trainingEvent;
        }

        public static void Delete(int trainingEventID)
        {
            throw new NotImplementedException();
        }

        public static void Update(TrainingEvent trainingEventEntity)
        {
            throw new NotImplementedException();
        }
    }
}
