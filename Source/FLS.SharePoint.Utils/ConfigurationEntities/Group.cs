using System.Collections.Generic;
using System.Xml.Serialization;

namespace FLS.SharePoint.Utils.ConfigurationEntities
{
    public class Group
    {
        [XmlAttribute]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<User> Users { get; set; }
    }
}
