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
        private readonly SPWeb currentWeb = SPContext.Current.Web;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var adminFlag = SPFarm.Local != null;
                var siteCollection = new List<SPSite>();
                if (adminFlag)
                {
                    AuthMessageLabel.Text = "You are administration Farm and you can see all farm sites.";
                    var service = SPFarm.Local.Services.GetValue<SPWebService>(string.Empty);
                    foreach (SPWebApplication webApplication in service.WebApplications)
                    {
                        siteCollection.AddRange(webApplication.Sites);
                    }
                }
                else
                {
                    AuthMessageLabel.Text = "You are not Farm administration and you can't see all farm sites, only current site and it children sites";
                    siteCollection.Add(SPContext.Current.Site);
                }

                ShowSiteCollection(siteCollection);

                if (siteCollectionTree.SelectedNode != null)
                {
                    ShowLists(siteCollectionTree.SelectedNode.Value, siteCollectionTree.SelectedNode.Text);
                }
            }
        }

        protected void SelectedNodeChanged(object sender, EventArgs e)
        {
            var node = siteCollectionTree.SelectedNode;
            var siteUrl = node.Value;
            ShowLists(siteUrl, node.Text);
        }

        private static void ShowWebCollection(SPWebCollection collection, Func<string, string, TreeNode, TreeNode> func, TreeNode rootNode)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                var info = collection.WebsInfo[i];

                var node = func.Invoke(info.Title, info.ServerRelativeUrl, rootNode);

                if (collection[i].Webs.Count > 0)
                {
                    ShowWebCollection(collection[i].Webs, func, node);
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

        private void ShowLists(string siteUrl, string siteTitle)
        {
            var siteLists = GetSiteLists(siteUrl).ToList();
            AvailableListsRpt.DataSource = siteLists;
            AvailableListsRpt.DataBind();

            SiteNameLabel.Text = string.Format("Selected site: {0}", siteTitle);
        }

        private void ShowSiteCollection(IEnumerable<SPSite> sites)
        {
            foreach (SPSite site in sites)
            {
                using (site)
                {
                    var rootWeb = site.RootWeb;
                    var node = new TreeNode(rootWeb.Title, rootWeb.Url);
                    if (rootWeb.Url == currentWeb.Url)
                    {
                        node.Selected = true;
                    }

                    if (rootWeb.Webs.Count > 0)
                    {
                        ShowWebCollection(
                            rootWeb.Webs, 
                            (title, url, rootNode) =>
                                                            {
                                                                var childNode = new TreeNode(title, new Uri(new Uri(site.Url), url).ToString());
                                                                if (currentWeb.Url.Contains(url))
                                                                {
                                                                    childNode.Selected = true;
                                                                    childNode.Expand();
                                                                }

                                                                rootNode.ChildNodes.Add(childNode);

                                                                return childNode;
                                                            },
                                                            node);
                    }

                    siteCollectionTree.Nodes.Add(node);
                }
            }
        }
    }
}
