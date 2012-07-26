using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FLS.Sharepoint.FileSearchConnector.OkatoModel
{
    public class OkatoService
    {
        public static OkatoEntity ReadItem(string id)
        {
            return ReadList().SingleOrDefault(c => c.Id.ToUpper().Equals(id.ToUpper()));
        }

        public static IEnumerable<OkatoEntity> ReadList()
        {
            var entityList = new List<OkatoEntity>();
            using (var reader = new StreamReader(@"c:\okato.txt", Encoding.GetEncoding(1251)))
            {
                string row;
                while ((row = reader.ReadLine()) != null)
                {
                    var readerData = row.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var entity = new OkatoEntity
                                     {
                                         Id = readerData[0],
                                         City = readerData.Length > 2 ? readerData[2] : readerData[1]
                                     };
                    entityList.Add(entity);
                }
            }

            return entityList;
        }
    }
} 
