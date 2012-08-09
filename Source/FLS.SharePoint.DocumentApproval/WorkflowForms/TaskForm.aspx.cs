using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

namespace DocumentApproval.WorkflowForms
{
    public partial class TaskForm : WebPartPage
    {
        protected string TaskStatus { get; private set; }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            MasterPageFile = SPContext.Current.Web.MasterUrl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnApprove.Click += OnBtnApproveClick;
            btnReject.Click += OnBtnRejectClick;
            btnCancel.Click += OnBtnCancelClick;
            btnOnAdvisement.Click += OnBtnAdvisementClick;
            if (SPContext.Current != null && SPContext.Current.ListItem != null)
              InitializeFormElemets();
        }

        private void OnBtnCancelClick(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void InitializeFormElemets()
        {
            var task = SPContext.Current.ListItem;

            if (task["DocumentStatus"].ToString() == "Created")
            {
                btnOnAdvisement.Visible = true;
            }

            if (task["DocumentStatus"].ToString() == "On advisement")
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
            }

            var workflowfTaskStatus = SPContext.Current.ListItem["DocumentStatus"];
            taskStatus.Value = workflowfTaskStatus != null ? workflowfTaskStatus.ToString() : string.Empty;
            var workflowTaskAssigned = SPContext.Current.ListItem["AssignedTo"];
            taskReqested.Value = workflowTaskAssigned != null ? workflowTaskAssigned.ToString() : string.Empty;
            var workflowTaskDate = SPContext.Current.ListItem["DueDate"];
            taskDueDate.Value = workflowTaskDate != null ? workflowTaskDate.ToString() : string.Empty;
            var workflowTaskComment = SPContext.Current.ListItem["Body"];
            taskComment.Value = workflowTaskComment != null ? workflowTaskComment.ToString() : string.Empty; 
        }

        private void OnBtnAdvisementClick(object sender, EventArgs e)
        {
            var task = SPContext.Current.ListItem;
            task["DocumentStatus"] = "On advisement";
            task["Body"] = taskComment.Value;
            SaveButton.SaveItem(SPContext.Current, false, string.Empty);
            CloseForm();
        }

        private void OnBtnApproveClick(object sender, EventArgs e)
        {
            var task = SPContext.Current.ListItem;
            task["DocumentStatus"] = "Approved";
            task["Body"] = taskComment.Value;
            SaveButton.SaveItem(SPContext.Current, false, string.Empty);
            CloseForm();
        }

        private void OnBtnRejectClick(object sender, EventArgs e)
        {
            var task = SPContext.Current.ListItem;
            task["DocumentStatus"] = "Rejected";    
            SaveButton.SaveItem(SPContext.Current, false, string.Empty);
            CloseForm();
        } 

        private void CloseForm()
        {
            if ((SPContext.Current != null) && SPContext.Current.IsPopUI)
            {
                Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                Context.Response.Flush();
                Context.Response.End();
            }
            else
            {
                var str = Page.Request.QueryString["Source"];
                if (!string.IsNullOrEmpty(str))
                {
                    SPUtility.Redirect(string.Empty, SPRedirectFlags.UseSource, Context);
                }
            }
        }
    }
}
