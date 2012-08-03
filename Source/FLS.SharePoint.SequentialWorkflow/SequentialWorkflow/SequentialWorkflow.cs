using System;
using System.Workflow.Activities;

using Microsoft.SharePoint.Workflow;

namespace FLS.SharePoint.SequentialWorkflow.SequentialWorkflow
{
    public sealed partial class SequentialWorkflow : SequentialWorkflowActivity
    {
        #region Constants and Fields

        private bool taskCompleted = false;
        public SPWorkflowTaskProperties taskProperties = new SPWorkflowTaskProperties();
        public SPWorkflowTaskProperties afterProperties = new SPWorkflowTaskProperties();
        public SPWorkflowTaskProperties beforeProperties = new SPWorkflowTaskProperties();
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public String taskContentTypeId = default(String);
        public String completeTaskOutcome = default(String);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialWorkflow"/> class.
        /// </summary>
        public SequentialWorkflow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void OnCreateTaskInvoke(object sender, EventArgs e)
        {
            this.taskProperties = new SPWorkflowTaskProperties();
            this.taskId = Guid.NewGuid();
            this.taskContentTypeId = "0x01080100FFbc98c2529347a5886b8d2576b954ef";
        }

        private void OnTaskChangedInvoke(object sender, ExternalDataEventArgs e)
        {
            this.taskCompleted = afterProperties.PercentComplete == 1F;
        }

        private void OnWhileInvoke(object sender, ConditionalEventArgs e)
        {
            e.Result = !this.taskCompleted;
        }

        #endregion

        public Guid taskId = default(System.Guid);
    }
}
