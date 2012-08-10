using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace FLS.SharePoint.EventReceiver.QAWorkflowEventReceiver
{
    /// <summary>
    /// List Workflow Events
    /// </summary>
    public class QAWorkflowEventReceiver : SPWorkflowEventReceiver
    {
       /// <summary>
       /// A workflow was completed.
       /// </summary>
       public override void WorkflowCompleted(SPWorkflowEventProperties properties)
       {
           base.WorkflowCompleted(properties);
       }


    }
}
