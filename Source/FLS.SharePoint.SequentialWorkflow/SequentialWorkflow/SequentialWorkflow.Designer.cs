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

namespace FLS.SharePoint.SequentialWorkflow.SequentialWorkflow
{
    public sealed partial class SequentialWorkflow
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
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            this.onTaskChanged = new Microsoft.SharePoint.WorkflowActions.OnTaskChanged();
            this.sequenceActivity = new System.Workflow.Activities.SequenceActivity();
            this.completeTask = new Microsoft.SharePoint.WorkflowActions.CompleteTask();
            this.whileActivity = new System.Workflow.Activities.WhileActivity();
            this.createTaskWithContentType = new Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType();
            this.onWorkflowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // onTaskChanged
            // 
            activitybind1.Name = "SequentialWorkflow";
            activitybind1.Path = "afterProperties";
            activitybind2.Name = "SequentialWorkflow";
            activitybind2.Path = "beforeProperties";
            correlationtoken1.Name = "taskToken";
            correlationtoken1.OwnerActivityName = "SequentialWorkflow";
            this.onTaskChanged.CorrelationToken = correlationtoken1;
            this.onTaskChanged.Executor = null;
            this.onTaskChanged.Name = "onTaskChanged";
            activitybind3.Name = "SequentialWorkflow";
            activitybind3.Path = "taskId";
            this.onTaskChanged.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.OnTaskChangedInvoke);
            this.onTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.AfterPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.onTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.BeforePropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.onTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // sequenceActivity
            // 
            this.sequenceActivity.Activities.Add(this.onTaskChanged);
            this.sequenceActivity.Name = "sequenceActivity";
            // 
            // completeTask
            // 
            this.completeTask.CorrelationToken = correlationtoken1;
            this.completeTask.Name = "completeTask";
            activitybind4.Name = "SequentialWorkflow";
            activitybind4.Path = "taskId";
            activitybind5.Name = "SequentialWorkflow";
            activitybind5.Path = "completeTaskOutcome";
            this.completeTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CompleteTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.completeTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CompleteTask.TaskOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // whileActivity
            // 
            this.whileActivity.Activities.Add(this.sequenceActivity);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.OnWhileInvoke);
            this.whileActivity.Condition = codecondition1;
            this.whileActivity.Name = "whileActivity";
            // 
            // createTaskWithContentType
            // 
            activitybind6.Name = "SequentialWorkflow";
            activitybind6.Path = "taskContentTypeId";
            this.createTaskWithContentType.CorrelationToken = correlationtoken1;
            this.createTaskWithContentType.ListItemId = -1;
            this.createTaskWithContentType.Name = "createTaskWithContentType";
            this.createTaskWithContentType.SpecialPermissions = null;
            activitybind7.Name = "SequentialWorkflow";
            activitybind7.Path = "taskId";
            activitybind8.Name = "SequentialWorkflow";
            activitybind8.Path = "taskProperties";
            this.createTaskWithContentType.MethodInvoking += new System.EventHandler(this.OnCreateTaskInvoke);
            this.createTaskWithContentType.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.ContentTypeIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.createTaskWithContentType.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.createTaskWithContentType.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // onWorkflowActivated
            // 
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "SequentialWorkflow";
            this.onWorkflowActivated.CorrelationToken = correlationtoken2;
            this.onWorkflowActivated.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated.Name = "onWorkflowActivated";
            activitybind9.Name = "SequentialWorkflow";
            activitybind9.Path = "workflowProperties";
            this.onWorkflowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // SequentialWorkflow
            // 
            this.Activities.Add(this.onWorkflowActivated);
            this.Activities.Add(this.createTaskWithContentType);
            this.Activities.Add(this.whileActivity);
            this.Activities.Add(this.completeTask);
            this.Name = "SequentialWorkflow";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated;

        private Microsoft.SharePoint.WorkflowActions.CompleteTask completeTask;

        private Microsoft.SharePoint.WorkflowActions.OnTaskChanged onTaskChanged;

        private SequenceActivity sequenceActivity;

        private WhileActivity whileActivity;

        private Microsoft.SharePoint.WorkflowActions.CreateTaskWithContentType createTaskWithContentType;














    }
}
