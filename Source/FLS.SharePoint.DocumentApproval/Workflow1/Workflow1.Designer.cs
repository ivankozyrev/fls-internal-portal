using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace DocumentApproval.Workflow1
{
    public sealed partial class Workflow1
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            this.onRequestTaskChanged = new Microsoft.SharePoint.WorkflowActions.OnTaskChanged();
            this.completeRequestTask = new Microsoft.SharePoint.WorkflowActions.CompleteTask();
            this.whileActivity1 = new System.Workflow.Activities.WhileActivity();
            this.createRequestTask = new Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // onRequestTaskChanged
            // 
            activitybind1.Name = "Workflow1";
            activitybind1.Path = "RequestTaskAfterProperties";
            activitybind2.Name = "Workflow1";
            activitybind2.Path = "RequestTaskBeforeProperties";
            correlationtoken1.Name = "RequestTaskToken";
            correlationtoken1.OwnerActivityName = "Workflow1";
            this.onRequestTaskChanged.CorrelationToken = correlationtoken1;
            this.onRequestTaskChanged.Executor = null;
            this.onRequestTaskChanged.Name = "onRequestTaskChanged";
            activitybind3.Name = "Workflow1";
            activitybind3.Path = "RequestApproveTaskId";
            this.onRequestTaskChanged.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.OnRequestTaskChanged_Invoked);
            this.onRequestTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.BeforePropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.onRequestTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.onRequestTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.AfterPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // completeRequestTask
            // 
            this.completeRequestTask.CorrelationToken = correlationtoken1;
            this.completeRequestTask.Name = "completeRequestTask";
            activitybind4.Name = "Workflow1";
            activitybind4.Path = "RequestApproveTaskId";
            this.completeRequestTask.TaskOutcome = null;
            this.completeRequestTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CompleteTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // whileActivity1
            // 
            this.whileActivity1.Activities.Add(this.onRequestTaskChanged);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.NotRequestTaskApproved);
            this.whileActivity1.Condition = codecondition1;
            this.whileActivity1.Name = "whileActivity1";
            // 
            // createRequestTask
            // 
            activitybind5.Name = "Workflow1";
            activitybind5.Path = "TaskContentTypeId";
            this.createRequestTask.CorrelationToken = correlationtoken1;
            activitybind6.Name = "Workflow1";
            activitybind6.Path = "RequestApproveTaskItemId";
            this.createRequestTask.Name = "createRequestTask";
            this.createRequestTask.SpecialPermissions = null;
            activitybind7.Name = "Workflow1";
            activitybind7.Path = "RequestApproveTaskId";
            activitybind8.Name = "Workflow1";
            activitybind8.Path = "RequestTaskProperties";
            this.createRequestTask.MethodInvoking += new System.EventHandler(this.CreateRequestTask_MethodInvoking);
            this.createRequestTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.ContentTypeIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.createRequestTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.createRequestTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.createRequestTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.ListItemIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            activitybind10.Name = "Workflow1";
            activitybind10.Path = "WorkflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "Workflow1";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken2;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind9.Name = "Workflow1";
            activitybind9.Path = "WorkflowProperties";
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // Workflow1
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.createRequestTask);
            this.Activities.Add(this.whileActivity1);
            this.Activities.Add(this.completeRequestTask);
            this.Name = "Workflow1";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType createRequestTask;

        private WhileActivity whileActivity1;

        private Microsoft.SharePoint.WorkflowActions.OnTaskChanged onRequestTaskChanged;

        private Microsoft.SharePoint.WorkflowActions.CompleteTask completeRequestTask;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;



















































    }
}
