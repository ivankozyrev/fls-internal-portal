using System;
using System.Workflow.Activities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace DocumentApproval.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            WorkflowId = default(Guid);
            WorkflowProperties = new SPWorkflowActivationProperties();
            RequestTaskProperties = new SPWorkflowTaskProperties();
            RequestTaskAfterProperties = new SPWorkflowTaskProperties();
            RequestTaskBeforeProperties = new SPWorkflowTaskProperties();
            RequestApproveTaskId = default(Guid);
            InitializeComponent();
        }

        public Guid WorkflowId { get; set; }

        public SPWorkflowActivationProperties WorkflowProperties { get; set; }

        public SPWorkflowTaskProperties RequestTaskBeforeProperties { get; set; }

        public SPWorkflowTaskProperties RequestTaskAfterProperties { get; set; }

        public SPWorkflowTaskProperties RequestTaskProperties { get; set; }

        public int RequestApproveTaskItemId { get; set; }

        public Guid RequestApproveTaskId { get; set; }

        public string TaskContentTypeId { get; set; }

        private bool RequestApproveComplete { get; set; }

        private SPList TaskList
        {
            get { return WorkflowProperties.Web.Lists["CustomTasks"]; }
        }

        private void CreateRequestTask_MethodInvoking(object sender, EventArgs e)
        {
            // Set up some of the properties.
            RequestApproveTaskId = Guid.NewGuid();
            TaskContentTypeId = TaskList.ContentTypes["Custom Tasks"].Id.ToString(); // "0x01080100FFbc98c2529347a5886b8d2576b954ef";
            RequestTaskProperties.Title = WorkflowProperties.Item[WorkflowConsts.WorkflowName] + " is ready for review";
            RequestTaskProperties.Description = "Please review the request and ensure it is valid.  If it is valid, then please select 'I Approve' on this task and save it.";
            RequestTaskProperties.ExtendedProperties[WorkflowConsts.TaskStatus] = WorkflowConsts.DocumentCreated;
            RequestTaskProperties.AssignedTo = WorkflowConsts.TaskAssignedUser;

            // Update document status
            var currentItem = WorkflowProperties.Item;
            currentItem[WorkflowConsts.TaskStatus] = WorkflowConsts.DocumentCreated;
            currentItem[WorkflowConsts.WorkflowStepConsolidatedNote] = string.Empty;
            currentItem.Update();
            LogComment("Request task was started", "Request Task Created.");
        }

        private void OnRequestTaskChanged_Invoked(object sender, ExternalDataEventArgs e)
        {
            var task = TaskList.GetItemById(RequestApproveTaskItemId);
            var taskComment = WorkflowHelper.ConvertFieldToString(task[WorkflowConsts.WorkflowStepConsolidatedNote]);

            switch (task[WorkflowConsts.TaskStatus].ToString())
            {
                case WorkflowConsts.DocumentOnAdvisement:
                    {
                        UpdateDocumentFields(WorkflowConsts.DocumentOnAdvisement, taskComment);
                        LogComment("Request task was moved to in progress", WorkflowConsts.DocumentOnAdvisement);
                        break;
                    }

                case WorkflowConsts.DocumentApproved:
                    {
                        UpdateDocumentFields(WorkflowConsts.DocumentApproved, taskComment);
                        LogComment("Request task was successful completed", WorkflowConsts.DocumentApproved);
                        RequestApproveComplete = true;
                        break;
                    }

                case WorkflowConsts.DocumentRejected:
                    {
                        UpdateDocumentFields(WorkflowConsts.DocumentRejected, taskComment);
                        LogComment("Request task was successful completed", WorkflowConsts.DocumentRejected);
                        RequestApproveComplete = true;
                        break;
                    }
            }
        }

        private void UpdateDocumentFields(string documentStatus, string comment)
        {
            WorkflowProperties.Item[WorkflowConsts.TaskStatus] = documentStatus;
            WorkflowProperties.Item[WorkflowConsts.WorkflowStepConsolidatedNote] = comment;
            WorkflowProperties.Item.Update();
        }

        private void LogComment(string description, string documentStatus)
        {
            SPWorkflow.CreateHistoryEvent(
                WorkflowProperties.Web,
                WorkflowInstanceId,
                0,
                WorkflowProperties.Web.CurrentUser,
                new TimeSpan(),
                description,
                documentStatus,
                string.Empty);
        }

        private void NotRequestTaskApproved(object sender, ConditionalEventArgs e)
        {
            e.Result = !RequestApproveComplete;
        }
    }
}
