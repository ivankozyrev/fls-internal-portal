using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace FLS.Sharepoint.ClientObjectModel.SharedListsWebPart
{
    [ToolboxItemAttribute(false)]
    public class SharedListsWebPart : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
      //  private const string _ascxPath = @"~/_CONTROLTEMPLATES/FLS.Sharepoint.ClientObjectModel/SharedListsWebPart/SharedListsWebPartUserControl.ascx";

        #region protected child control variable definitions

        private ListViewByQuery viewByQuery;
        private EncodedLiteral encodedLiteral;
        #endregion

        [Personalizable, WebPartStorage(Storage.Shared), WebBrowsable, WebDisplayName("View Name"), WebDescription("View Name")]
        public string ViewName { get; set; }

        [Personalizable, WebPartStorage(Storage.Shared), WebBrowsable, WebDisplayName("Site Url"), WebDescription("Site Url")]
        public string SiteUrl { get; set; }

        [Personalizable, WebPartStorage(Storage.Shared), WebBrowsable, WebDisplayName("Source List"), WebDescription("Source list to query")]
        public string SourceList { get; set; }

        [Personalizable, WebPartStorage(Storage.Shared), WebBrowsable, WebDisplayName("Disable Filter"), WebDescription("Disable List Filtering")]
        public bool DisableFilter { get; set; }

        [Personalizable, WebPartStorage(Storage.Shared), WebBrowsable, WebDisplayName("Disable Sort"), WebDescription("Disable list sorting")]
        public bool DisableSort { get; set; }

        public SharedListsWebPart()
        {
            ViewName = string.Empty;
            SiteUrl = string.Empty;
            SourceList = string.Empty;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            SPSite site = null; 
             SPWeb web = null;
            bool disposeSPSite = false;

            try
            {
                if (SourceList != string.Empty && SourceList != string.Empty)
                 {
                     viewByQuery = new ListViewByQuery();

                     if (!string.IsNullOrEmpty(SiteUrl))
                     {
                         site = new SPSite(SiteUrl);
                         disposeSPSite = true;
                     }
                     else
                     {
                         site = SPContext.Current.Site;
                     }

                     web = site.OpenWeb();
                     SPList sourceList = web.Lists[SourceList];
                     viewByQuery.List = sourceList;
                     SPQuery query = null;

                     query = CheckIfViewExists(viewByQuery.List)
                         ? new SPQuery(viewByQuery.List.Views[ViewName]) 
                         : new SPQuery(viewByQuery.List.DefaultView);

                     viewByQuery.Query = query;
                     viewByQuery.DisableFilter = DisableFilter;
                     viewByQuery.DisableSort = DisableSort;
                     Controls.Add(viewByQuery);
                 }
                 else
                 {
                     encodedLiteral = new EncodedLiteral { Text = "This webpart is not configured." };
                     Controls.Add(encodedLiteral);
                 }
            }
            finally
            {
                if (disposeSPSite)
                {
                    if (site != null)
                    {
                        ((IDisposable)site).Dispose();
                    }

                    if (web != null)
                    {
                        ((IDisposable)web).Dispose();
                    }
                }
            }
        }

        private bool CheckIfViewExists(SPList list)
         {
             bool ret = false;
  
             foreach (SPView view in list.Views)
             {
                 if (view.Title.ToLower() == ViewName.ToLower())
                 {
                     ret = true;
                 }
             }

             return ret;
         }
    }
}
