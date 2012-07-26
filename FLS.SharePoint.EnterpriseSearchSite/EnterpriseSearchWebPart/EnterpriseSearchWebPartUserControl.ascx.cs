using System;
using System.Data;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Search.Query;

namespace FLS.SharePoint.EnterpriseSearchSite.EnterpriseSearchWebPart
{
    public partial class EnterpriseSearchWebPartUserControl : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            Load += OnLoad;
            base.OnInit(e);
        }

        protected void OnBtnSearchClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtQ.Value))
            {
                ExecuteSearchQuery(txtQ.Value);
            }
            else
            {
                txtErrorMessage.InnerText = @"There is nothing to search.";
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            btnSearch.ServerClick += OnBtnSearchClick;
            txtErrorMessage.InnerText = string.Empty;
        }

        private void ExecuteSearchQuery(string query)
        {
            using (var site = new SPSite(SPContext.Current.Web.Site.Url))
            {
                var keywordQuery = new KeywordQuery(site)
                                       {
                                           QueryText = query, 
                                           ResultTypes = ResultType.RelevantResults, 
                                           KeywordInclusion = KeywordInclusion.AnyKeyword
                                       };
                var resultTableCollection = keywordQuery.Execute();
                if (resultTableCollection.Count > 0)
                {
                    var relevantResultTable = resultTableCollection[ResultType.RelevantResults];
                    var search = new DataTable();
                    search.Load(relevantResultTable);
                    grdResults.DataSource = search;
                    grdResults.DataBind();
                }
            }
        }
    }
}
