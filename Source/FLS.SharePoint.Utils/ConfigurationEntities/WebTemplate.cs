using System.Xml.Serialization;

namespace FLS.SharePoint.Utils.ConfigurationEntities
{
    public class WebTemplate
    {
        [XmlAttribute]
        public string Name { get; set; }

        public string PackageFileName { get; set; }
    }
}
