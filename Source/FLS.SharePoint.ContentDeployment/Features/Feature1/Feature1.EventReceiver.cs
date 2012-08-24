using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FLS.SharePoint.ContentDeployment.Features.Feature1
{
    [Guid("c7f50323-6085-4a3a-9ac3-33f7ad52da8c")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
      private const string ExportJobName = "ExportContenJob";
      private const string ImportJobName = "ImportContenJob";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
           var site = properties.Feature.Parent as SPSite;
            DeleteJob(ExportJobName, site);
            var exportJob = new ExportContentJob(ExportJobName, site.WebApplication);
            exportJob.IsDisabled = true;
            exportJob.Schedule = CreateShedule();
            exportJob.Update();

            DeleteJob(ImportJobName, site);
            var importJob = new ImportContentJob(ImportJobName, site.WebApplication);
            importJob.IsDisabled = true;
            importJob.Schedule = CreateShedule();
            importJob.Update();      
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            
            DeleteJob(ExportJobName, site);
            DeleteJob(ImportJobName, site);
        }

        private SPMinuteSchedule CreateShedule()
        {
            var schedule = new SPMinuteSchedule();
            schedule.BeginSecond = 0;
            schedule.EndSecond = 59;
            schedule.Interval = 5;

            return schedule;
        }

        private void DeleteJob(string jobName, SPSite site)
        {
            foreach (var job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == jobName) job.Delete();
            }
        }
    }
}
