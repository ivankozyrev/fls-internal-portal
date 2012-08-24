using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Deployment;

namespace FLS.SharePoint.ContentDeployment
{
    public class ImportContentJob : SPJobDefinition
    {
         public ImportContentJob() 
               : base()
        {
        }

          public ImportContentJob(string jobName, SPService service, SPServer server, SPJobLockType targetType)

            : base(jobName, service, server, targetType)
        {

        }

          public ImportContentJob(string jobName, SPWebApplication webApplication)

            : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            Title = "Import content job";
        }

        public override void Execute(System.Guid targetInstanceId)
        {
            var settings = new SPImportSettings();

            settings.SiteUrl = "http://oprikhodko/sites/production";
            settings.FileLocation = @"C:\export";
            settings.BaseFileName = "blop.cmp";
            settings.FileCompression = true;
            settings.RetainObjectIdentity = false;
            settings.SuppressAfterEvents = true;
            settings.UserInfoDateTime = SPImportUserInfoDateTimeOption.ImportAll;
            settings.UpdateVersions = SPUpdateVersions.Overwrite;


            var import = new SPImport(settings);
            import.Run();
        }

    }
}
