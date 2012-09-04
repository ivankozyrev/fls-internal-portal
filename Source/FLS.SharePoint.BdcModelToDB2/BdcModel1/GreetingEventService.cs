using System.Collections.Generic;
using System.Data;
using IBM.Data.DB2;

namespace FLS.SharePoint.BdcModelToDB2.BdcModel1
{
    /// <summary>
    /// All the methods for retrieving, updating and deleting data are implemented in this class file.
    /// The samples below show the finder and specific finder method for Entity1.
    /// </summary>
    public class GreetingEventService
    {
        private static DB2Connection GetSqlConnection()
        {
            return new DB2Connection("Database=Test2;UserID=user;Password=password;Server=localhost:50000");
        }
        /// <summary>
        /// This is a sample specific finder method for Entity1.
        /// If you want to delete or rename the method think about changing the xml in the BDC model file as well.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity1</returns>
        public static GreetingEvent ReadItem(string id)
        {
            var greetingEvent = new GreetingEvent();
            var db2Connection = GetSqlConnection();
            db2Connection.Open();
            var sqlCommand = new DB2Command
            {
                CommandText = string.Format("SELECT * FROM GREETING WHERE deptno = '{0}'", id),
                Connection = db2Connection
            };
            var sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            if (sqlDataReader.Read())
            {
                greetingEvent.DeptNo = sqlDataReader[0].ToString();
                greetingEvent.Greeting = sqlDataReader[1].ToString();
            }
            db2Connection.Dispose();
            return greetingEvent;
        }
        /// <summary>
        /// This is a sample finder method for Entity1.
        /// If you want to delete or rename the method think about changing the xml in the BDC model file as well.
        /// </summary>
        /// <returns>IEnumerable of Entities</returns>
        public static IEnumerable<GreetingEvent> ReadList()
        {
            var db2Connection = GetSqlConnection();
            var trainingEvents = new List<GreetingEvent>();
            db2Connection.Open();
            var db2Command = new DB2Command
            {
                Connection = db2Connection,
                CommandText = "SELECT * FROM GREETING",
            };
            var sqlDataReader = db2Command.ExecuteReader(CommandBehavior.CloseConnection);
            while (sqlDataReader.Read())
            {
                var trainingEvent = new GreetingEvent
                {
                    DeptNo = sqlDataReader[0].ToString(),
                    Greeting = sqlDataReader[1].ToString()
                };
                trainingEvents.Add(trainingEvent);
            }

            db2Connection.Dispose();
            return trainingEvents;
        }
    }
}
