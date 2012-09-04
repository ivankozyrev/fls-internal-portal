using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace DocumentApproval.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("733e31cc-540a-4a72-a0b2-3de02caa4d1e")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (var site = GetSite(properties)) //Change your web app
            {
                site.AllowUnsafeUpdates = true;

                using (var web = site.OpenWeb())
                {
                    var d = new SiteEntitiesDataContext(site.Url);
                    var list2 = d.HRLibrary;
                    var list = web.Lists["HR Library"];
                    //List which we is going to associate the workflow           

                    // Try to get workflow history list
                    SPList historyList;
                    try
                    {
                        historyList = web.Lists["Workflow History"];
                    }
                    catch (ArgumentException exc)
                    {
                        // Create workflow history list
                        var listGuid = web.Lists.Add("Workflow History", string.Empty, SPListTemplateType.WorkflowHistory);
                        historyList = web.Lists[listGuid];
                        historyList.Hidden = true;
                        historyList.Update();
                    }

                    // Try to get workflow tasks list
                    SPList taskList; 
                    try
                    {
                        taskList = web.Lists["CustomTasks"];
                    }
                    catch (ArgumentException exc)
                    {
                        // Create workflow tasks list
                        var listGuid = web.Lists.Add("CustomTasks", "CustomTasks", SPListTemplateType.Tasks);
                        taskList = web.Lists[listGuid];
                        taskList.Hidden = true;
                        taskList.Update();
                    }

                    SPWorkflowAssociation workflowAssociation;
                    try
                    {
                        // Create workflow association
                        var workflowTemplate = web.WorkflowTemplates[new Guid("0b070921-3ce6-4694-8d87-f2f4ab096cc7")];
                        workflowAssociation = SPWorkflowAssociation.CreateListContentTypeAssociation(workflowTemplate,
                                                                                            "Document Approval",
                                                                                            taskList, historyList);
                        // Set workflow parameters 
                        workflowAssociation.AllowManual = true;
                        workflowAssociation.AutoStartCreate = true;
                        workflowAssociation.AutoStartChange = false;

                        // Add workflow association to my list
                        list.WorkflowAssociations.Add(workflowAssociation);

                        // Enable workflow
                        workflowAssociation.Enabled = true;
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = false;
                    }
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            using (var site = GetSite(properties)) //Change your web app
            {
                using (var web = site.OpenWeb())
                {

                    var list = web.Lists["HR Library"];
                    var workflowAssociation = list.WorkflowAssociations.GetAssociationByName("Document Approval", CultureInfo.CurrentCulture);
                    if (workflowAssociation != null)
                        list.WorkflowAssociations.Remove(workflowAssociation);

                    list.Update();
                }
            }
        }

        private static SPSite GetSite(SPFeatureReceiverProperties properties)
        {
            SPSite site;
            if (properties.Feature.Parent is SPWeb)
            {
                site = ((SPWeb)properties.Feature.Parent).Site;
            }
            else if (properties.Feature.Parent is SPSite)
            {
                site = (SPSite)properties.Feature.Parent;
            }
            else
            {
                throw new Exception("Unable to retrieve SPSite - this feature is not Site or Web-scoped.");
            }

            return site;
        }
    }
}
