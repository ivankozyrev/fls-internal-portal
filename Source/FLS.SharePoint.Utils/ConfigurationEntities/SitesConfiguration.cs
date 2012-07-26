using System.Collections.Generic;

namespace FLS.SharePoint.Utils.ConfigurationEntities
{
    public class SitesConfiguration
    {
        public string SiteOwner { get; set; }
        
        public List<Site> Sites { get; set; }

        public List<Group> Groups { get; set; }
    }
}
