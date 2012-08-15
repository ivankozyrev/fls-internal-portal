using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

namespace DocumentApproval.WorkflowForms
{
    public partial class TaskForm : WebPartPage
    {
        private SPListItem currentTask;

        private SPListItem CurrentTask
        {
            get { return currentTask ?? (currentTask = SPContext.Current.ListItem); }
        }

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

            if (CurrentTask != null)
            {
                InitializeFormElements();
            }
        }

        private void InitializeFormElements()
        {
            var task = CurrentTask;

            if (!IsPostBack)
            {
                taskStatus.Value = WorkflowHelper.ConvertFieldToString(task[WorkflowConsts.TaskStatus]);
                taskReqested.Value = WorkflowHelper.ConvertFieldToString(task[WorkflowConsts.TaskAssignedTo]);
                taskDueDate.Value = WorkflowHelper.ConvertFieldToString(task[WorkflowConsts.TaskDueDate]);
                taskComment.Value = WorkflowHelper.ConvertFieldToString(task[WorkflowConsts.WorkflowStepConsolidatedNote]); 
            }

            // task statuses buttons
            if (task[WorkflowConsts.TaskStatus].ToString() == WorkflowConsts.DocumentCreated)
            {
                btnOnAdvisement.Visible = true;
            }

            if (task[WorkflowConsts.TaskStatus].ToString() == WorkflowConsts.DocumentOnAdvisement)
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
            }
        }

        private void OnBtnAdvisementClick(object sender, EventArgs e)
        {
            InitTaskFields(WorkflowConsts.DocumentOnAdvisement, taskComment.Value);
            SaveButton.SaveItem(SPContext.Current, false, string.Empty);
            CloseForm();
        }

        private void OnBtnApproveClick(object sender, EventArgs e)
        {
            InitTaskFields(WorkflowConsts.DocumentApproved, taskComment.Value);
            SaveButton.SaveItem(SPContext.Current, false, string.Empty);
            CloseForm();
        }

        private void OnBtnRejectClick(object sender, EventArgs e)
        {
            InitTaskFields(WorkflowConsts.DocumentRejected, taskComment.Value);
            SaveButton.SaveItem(SPContext.Current, false, string.Empty);
            CloseForm();
        }

        private void OnBtnCancelClick(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void InitTaskFields(string documentStatus, string comment)
        {
            var task = CurrentTask;
            task[WorkflowConsts.TaskStatus] = documentStatus;
            task[WorkflowConsts.WorkflowStepConsolidatedNote] = comment;
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
