using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Deployment;

namespace FLS.SharePoint.ContentDeployment
{
    public class ExportContentJob : SPJobDefinition
    {
        public ExportContentJob() 
               : base()
        {
        }

          public ExportContentJob(string jobName, SPService service, SPServer server, SPJobLockType targetType)

            : base(jobName, service, server, targetType)
        {
        }

        public ExportContentJob(string jobName, SPWebApplication webApplication)

            : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            Title = "Export content job";
        }

        public override void Execute(System.Guid targetInstanceId)
        {
            var settings = new SPExportSettings();
            settings.SiteUrl = "http://oprikhodko/sites/workflow";
            settings.ExportMethod = SPExportMethodType.ExportAll;
            settings.FileLocation = @"c:\export";
           // settings.FileCompression = true;
            settings.CommandLineVerbose = true;
            settings.BaseFileName = "blop";
            settings.OverwriteExistingDataFile = true;

            var export = new SPExport(settings);
            export.Run(); 
        }
    }
}
