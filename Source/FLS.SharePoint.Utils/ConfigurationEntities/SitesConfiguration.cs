using System.Collections.Generic;

namespace FLS.SharePoint.Utils.ConfigurationEntities
{
    public class SitesConfiguration
    {
        public string SitesOwner { get; set; }

        public string TemplatesDirectory { get; set; }

        public List<Site> Sites { get; set; }

        public List<Group> Groups { get; set; }
    }
}
