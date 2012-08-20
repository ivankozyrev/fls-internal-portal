using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Deployment;

namespace FLS.SharePoint.DocumentApproval
{
    public class ImportContentJob : SPJobDefinition
    {
        public ImportContentJob() 
               : base()
        {
        }

        public override void Execute(System.Guid targetInstanceId)
        {
            var settings = new SPExportSettings();
            settings.SiteUrl = "http://localhost/sites/authoring";
            settings.ExportMethod = SPExportMethodType.ExportAll;
            settings.FileLocation = @"c:\export";
            settings.FileCompression = false;
            settings.CommandLineVerbose = true;

            var export = new SPExport(settings);
            export.Run(); 
        }
    }
}
