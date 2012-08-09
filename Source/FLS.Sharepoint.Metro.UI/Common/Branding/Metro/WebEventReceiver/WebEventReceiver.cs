using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace FLS.Sharepoint.Metro.UI.Common.Branding.Metro
{
    public class WebEventReceiver : SPWebEventReceiver
    {
        private const string MasterPagePath = @"_catalogs/masterpage/";
        private readonly string[] _searchWebTemplates = new[] { "SRCHCENTERLITE", "SRCHCEN", "SRCHCENTERFAST" };
        private string _masterPageUrl = "Metro.master";
        private string _searchMasterPageUrl = "MetroSearch.master";

        public override void WebProvisioned(SPWebEventProperties properties)
        {
            var web = properties.Web;
            var rootWeb = web.Site.RootWeb;
            _masterPageUrl = SPUrlUtility.CombineUrl(rootWeb.ServerRelativeUrl, MasterPagePath + _masterPageUrl);
            _searchMasterPageUrl = SPUrlUtility.CombineUrl(rootWeb.ServerRelativeUrl, MasterPagePath + _searchMasterPageUrl);

            web.MasterUrl = _masterPageUrl;
            web.CustomMasterUrl = _searchWebTemplates.Contains(web.WebTemplate) ? _searchMasterPageUrl : _masterPageUrl;
            web.Update();
        }
    }
}
