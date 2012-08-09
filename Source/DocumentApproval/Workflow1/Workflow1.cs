using System;
using System.Workflow.Activities;
using Microsoft.SharePoint.Workflow;

namespace DocumentApproval.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        // these properties are for the workflow task that will be created.
        public SPWorkflowTaskProperties RequestTaskProperties = new SPWorkflowTaskProperties();
        public SPWorkflowTaskProperties RequestTaskBeforeProperties = new SPWorkflowTaskProperties();
        public SPWorkflowTaskProperties RequestTaskAfterProperties = new SPWorkflowTaskProperties();
        public Guid RequestApproveTaskId = default(Guid);
        public int RequestApproveTaskItemId;
        public bool RequestApproveComplete;
        public String taskContentTypeId = default(String);

        private void createRequestTask_MethodInvoking(object sender, EventArgs e)
        {
            // Set up some of the properties.
            RequestApproveTaskId = Guid.NewGuid();
            taskContentTypeId = "0x01080100FFbc98c2529347a5886b8d2576b954ef";
            RequestTaskProperties.Title = workflowProperties.Item["Имя"].ToString() + " is ready for review";
            RequestTaskProperties.Description = "Please review the request and ensure it is valid.  If it is valid, then please select 'I Approve' on this task and save it.";
            RequestTaskProperties.ExtendedProperties["DocumentStatus"] = "Created";

            //  Update document status
            var currentItem = workflowProperties.Item;
            currentItem["DocumentStatus"] = "Created";
            currentItem.Update();
            LogComment("Request task was started", "Request Task Created.");
        }

        private void onRequestTaskChanged_Invoked(object sender, ExternalDataEventArgs e)
        {
            var task = workflowProperties.Web.Lists["CustomTasks"].GetItemById(RequestApproveTaskItemId);

            switch (task["DocumentStatus"].ToString())
            {
                case "On advisement":
                    {
                        workflowProperties.Item["DocumentStatus"] = "On advisement";
                        workflowProperties.Item.Update();
                        LogComment("Request task was moved to in progress", "On advisement");
                        break;
                    }

                case "Approved":
                    {
                        workflowProperties.Item["DocumentStatus"] = "Approved";
                        workflowProperties.Item.Update();
                        LogComment("Request task was successful completed", "Approved");
                        RequestApproveComplete = true;
                        break;
                    }

                case "Rejected":
                    {
                        workflowProperties.Item["DocumentStatus"] = "Rejected";
                        workflowProperties.Item.Update();
                        LogComment("Request task was successful completed", "Rejected");
                        RequestApproveComplete = true;
                        break;
                    }
            }
        }

        private void LogComment(string description, string documentStatus)
        {
            SPWorkflow.CreateHistoryEvent(
                workflowProperties.Web,
                WorkflowInstanceId,
                0,
                workflowProperties.Web.CurrentUser,
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
