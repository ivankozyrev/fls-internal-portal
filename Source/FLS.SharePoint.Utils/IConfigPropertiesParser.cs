using FLS.SharePoint.Utils.ConfigurationEntities;

namespace FLS.SharePoint.Utils
{
    public interface IConfigPropertiesParser
    {
        SitesConfiguration ParseSitesConfiguration(string xmlConfigurationString);
    }
}