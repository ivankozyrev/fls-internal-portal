using System.IO;
using System.Xml;
using System.Xml.Serialization;
using FLS.SharePoint.Utils.ConfigurationEntities;

namespace FLS.SharePoint.Utils
{
    public class ConfigPropertiesParser : IConfigPropertiesParser
    {
        public SitesConfiguration ParseSitesConfiguration(string xmlConfigurationString)
        {
            var serializer = new XmlSerializer(typeof(SitesConfiguration));
            var xmlReader = new XmlTextReader(new StringReader(xmlConfigurationString));
            return (SitesConfiguration)serializer.Deserialize(xmlReader);
        }
    }
}
