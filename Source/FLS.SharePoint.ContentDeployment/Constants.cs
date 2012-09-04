namespace FLS.SharePoint.ContentDeployment
{
    public static class Constants
    {
        public const string MigrationContentJobName = "MigrationContentJob";
        public const string MigrationContentJobTitle = "Migration Content Job";
        public const string SettingsFilePath = "template\\layouts\\FLS.SharePoint.ContentDeployment\\migration.config";
        public const string ImportSettingElementName = "import";
        public const string ExportSettingElementName = "export";
        public const string RootSettingsElementName = "configuration";
        public const string SourceSiteElementName = "sourceUrl";
        public const string DestinationSiteElementName = "destinationUrl";
        public const string DefaultSiteUrl = "http://";
        public const string BaseFileName = "blop.cmp";
        public const string BaseFileNameElementName = "baseFileName";
        public const string ExportMethodElementName = "exportMethod";
        public const string UserInfoDateTimeElementName = "userInfoDateTime";
        public const string UpdateVersionsElementName = "updateVersions";
        public const string FileLocationElementName = "fileLocation";
        public const string DefaultTemporaryMigrationFolder = "ContentDeployment";
    }
}
