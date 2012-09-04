using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FLS.SharePoint.ContentDeployment.Features.Feature1
{
    [Guid("c7f50323-6085-4a3a-9ac3-33f7ad52da8c")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            if (site == null)
            {
                return;
            }

            DeleteJob(site.WebApplication, Constants.MigrationContentJobName);
            var migrationContentJob = new MigrationContentJob(Constants.MigrationContentJobName, site.WebApplication);
            migrationContentJob.Update();
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            if (site == null)
            {
                return;
            }

            DeleteJob(site.WebApplication, Constants.MigrationContentJobName);
        }

        private static void DeleteJob(SPWebApplication application, string jobName)
        {
            var job = application.JobDefinitions.SingleOrDefault(c => jobName == c.Name);
            if (job != null)
            {
                job.Delete();     
            }
        }
    }
}
