using System.Collections.Generic;
using System.Xml.Serialization;

namespace FLS.SharePoint.Utils.ConfigurationEntities
{
    public class Site
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        public string Description { get; set; }

        public WebTemplate WebTemplate { get; set; }

        public List<Permission> Permissions { get; set; }
    }
}
