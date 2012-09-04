using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Deployment;

namespace FLS.SharePoint.ContentDeployment
{
    public class MigrationContentJob : SPJobDefinition
    {
        private readonly ConfigurationHelper _configurationHelper = new ConfigurationHelper();

        public MigrationContentJob()
        {
        }

        public MigrationContentJob(string jobName, SPService service, SPServer server, SPJobLockType targetType)
            : base(jobName, service, server, targetType)
        {
        }

        public MigrationContentJob(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            Title = Constants.MigrationContentJobTitle;
            Schedule = new SPWeeklySchedule { BeginDayOfWeek = DayOfWeek.Sunday, EndDayOfWeek = DayOfWeek.Sunday, BeginHour = 21, EndHour = 23 };
        }

        public override void Execute(Guid targetInstanceId)
        {
            var export = new SPExport(SetUpExportSettings());
            var import = new SPImport(SetUpImportSettings());
            export.Run();
            import.Run();
        }

        public SPExportSettings SetUpExportSettings()
        {
            return new SPExportSettings
            {
                SiteUrl = ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ExportSection, Constants.SourceSiteElementName),
                ExportMethod = ConfigurationHelper.GetMigrationConfigItem<SPExportMethodType>(_configurationHelper.ExportSection, Constants.ExportMethodElementName),
                FileLocation = ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ExportSection, Constants.FileLocationElementName),
                CommandLineVerbose = true,
                BaseFileName = ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ExportSection, Constants.BaseFileNameElementName),
                OverwriteExistingDataFile = true
            };
        }

        public SPImportSettings SetUpImportSettings()
        {
            return new SPImportSettings
            {
                SiteUrl = ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ImportSection, Constants.DestinationSiteElementName),
                FileLocation = ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ImportSection, Constants.FileLocationElementName),
                BaseFileName = ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ImportSection, Constants.BaseFileNameElementName),
                FileCompression = true,
                RetainObjectIdentity = false,
                SuppressAfterEvents = true,
                UserInfoDateTime = ConfigurationHelper.GetMigrationConfigItem<SPImportUserInfoDateTimeOption>(_configurationHelper.ImportSection, Constants.UserInfoDateTimeElementName),
                UpdateVersions = ConfigurationHelper.GetMigrationConfigItem<SPUpdateVersions>(_configurationHelper.ImportSection, Constants.UpdateVersionsElementName)
            };
        }
    }
}
