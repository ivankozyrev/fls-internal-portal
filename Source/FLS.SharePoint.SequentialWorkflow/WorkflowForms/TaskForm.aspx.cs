using System;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

namespace FLS.SharePoint.SequentialWorkflow.WorkflowForms
{
    public partial class TaskForm : WebPartPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.MasterPageFile = SPContext.Current.Web.MasterUrl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveAsDraft.Click += OnBtnSaveAsDraftClick;
            btnComplete.Click += OnBtnCompleteClick;
            btnCancel.Click += OnBtnCancelClick;
        }

        private void OnBtnCancelClick(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void CloseForm()
        {
            if ((SPContext.Current != null) && SPContext.Current.IsPopUI)
            {
                this.Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                this.Context.Response.Flush();
                this.Context.Response.End();
            }
            else
            {
                string str = this.Page.Request.QueryString["Source"];
                if (!string.IsNullOrEmpty(str))
                {
                    SPUtility.Redirect(string.Empty, SPRedirectFlags.UseSource, this.Context);
                }
            }
        }

        private void OnBtnCompleteClick(object sender, EventArgs e)
        {
            var li = SPContext.Current.ListItem;
            li[SPBuiltInFieldId.TaskStatus] = "Tasks_Completed";
            li[SPBuiltInFieldId.PercentComplete] = 1;

            SaveButton.SaveItem(SPContext.Current, false, string.Empty);

            CloseForm();
        }

        private void OnBtnSaveAsDraftClick(object sender, EventArgs e)
        {
            SaveButton.SaveItem(SPContext.Current, false, string.Empty);

            CloseForm();
        }
    }
}