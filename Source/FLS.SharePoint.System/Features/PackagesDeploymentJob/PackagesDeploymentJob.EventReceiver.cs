using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace FLS.SharePoint.System.Features.PackagesDeploymentJob
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("83be8dad-9337-4de9-978d-875d43f6dd2d")]
    public class PackagesDeploymentJobEventReceiver : SPFeatureReceiver
    {
        const string jobName = "ListLogger";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;

            // make sure the job isn't already registered
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {

                if (job.Name == jobName)
                    job.Delete();
            }

            // install the job

            var deploymentJob = new System.PackagesDeploymentJob(jobName, site.WebApplication);

            var schedule = new SPMinuteSchedule
                                            {
                                                BeginSecond = 0, EndSecond = 59, Interval = 5
                                            };

            deploymentJob.Schedule = schedule;

            deploymentJob.Update();
        }


        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;

            // make sure the job isn't already registered
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {

                if (job.Name == jobName)
                    job.Delete();
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
