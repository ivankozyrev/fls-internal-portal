using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client;

namespace FLS.Sharepoint.ClientObjectModel.AvailableSiteLists
{
    public partial class AvailableSiteListsUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var service = SPFarm.Local.Services.GetValue<SPWebService>(string.Empty);
                foreach (SPWebApplication webApplication in service.WebApplications)
                {
                    ShowSiteCollection(webApplication.Sites);
                }

                siteCollectionTree.CollapseAll();
            }
        }

        protected void SelectedNodeChanged(object sender, EventArgs e)
        {
            var node = siteCollectionTree.SelectedNode;
            var siteUrl = node.Value;
            var siteLists = GetSiteLists(siteUrl).ToList();
            AvailableListsRpt.DataSource = siteLists;
            AvailableListsRpt.DataBind();

            SiteNameLabel.Text = string.Format("Selected site: {0}", node.Text);
        }

        private static void ShowWebCollection(SPWebCollection collection, Action<string, string> func)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                var info = collection.WebsInfo[i];

                func.Invoke(info.Title, info.ServerRelativeUrl);

                if (collection[i].Webs.Count > 0)
                {
                    ShowWebCollection(collection[i].Webs, func);
                }
            }
        }

        private static IEnumerable<List> GetSiteLists(string siteUrl)
        {
            // TODO possible context should be have the one instance for each site
            using (var context = new ClientContext(siteUrl))
            {
                var query = from lists in context.Web.Lists
                            where !lists.Hidden
                            select lists;
                var siteLists = context.LoadQuery(query);
                context.ExecuteQuery();

                return siteLists;
            }
        }

        private void ShowSiteCollection(IEnumerable<SPSite> sites)
        {
            foreach (SPSite site in sites)
            {
                using (site)
                {
                    var rootWeb = site.RootWeb;
                    var node = new TreeNode(rootWeb.Title, rootWeb.Url);
                    if (rootWeb.Webs.Count > 0)
                    {
                        ShowWebCollection(rootWeb.Webs, (title, url) => node.ChildNodes.Add(new TreeNode(title,  new Uri(new Uri(site.Url), url).ToString())));
                    }

                    siteCollectionTree.Nodes.Add(node);
                }
            }
        }
    }
}
