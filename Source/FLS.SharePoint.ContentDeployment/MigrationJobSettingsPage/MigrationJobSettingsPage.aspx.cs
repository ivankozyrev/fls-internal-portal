using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.ApplicationPages;
using Microsoft.SharePoint.Deployment;
using Microsoft.SharePoint.WebControls;

namespace FLS.SharePoint.ContentDeployment
{
    public partial class MigrationJobSettingsPage : GlobalAdminPageBase
    {
        private readonly string _defaultMigrationFolder = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Constants.DefaultTemporaryMigrationFolder);

        private ConfigurationHelper _configurationHelper;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            BtnOk.Click += OnBtnOkClick;
            BtnCancel.Click += OnBtnCancelClick;
            WebAppSelector.ContextChange += WebAppSelectorOnContextChange;
            
            GetConfiguration();
            SetMigrationOptions();
            SetDefaultSettings();
            DataBindAllControls();
        }

        private void GetConfiguration()
        {
            _configurationHelper = new ConfigurationHelper();
        }

        private void DataBindAllControls()
        {
            ////foreach (Control control in Controls)
            ////{
            ////    control.DataBind();
            ////}
            ExportTypes.DataBind();
            ExportObjects.DataBind();
        }

        private void SetDefaultSettings()
        {
            ExportTypes.SelectedValue = ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ExportSection, Constants.ExportMethodElementName);
        }

        private void SetMigrationOptions()
        {
            try
            {
                var allExportObjects = new[]
                {
                    SPDeploymentObjectType.Site.ToString(),
                    SPDeploymentObjectType.Web.ToString(),
                    SPDeploymentObjectType.List.ToString()
                };
                ExportObjects.Items.AddRange(allExportObjects.Select(p => new ListItem(p)).ToArray());
                

                var availableExportTypes = new[]
                {
                    SPExportMethodType.ExportAll.ToString(),
                    SPExportMethodType.ExportChanges.ToString()
                };
                ExportTypes.Items.AddRange(availableExportTypes.Select(p => new ListItem(p)).ToArray());

                var defaultMigrationFolder = ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ExportSection, Constants.FileLocationElementName);
                FileLocation.Text = !string.IsNullOrEmpty(defaultMigrationFolder)
                                        ? defaultMigrationFolder
                                        : _defaultMigrationFolder;

                var defaultMigrationFileName = ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ExportSection, Constants.BaseFileNameElementName);
                FileName.Text = !string.IsNullOrEmpty(defaultMigrationFileName)
                                    ? defaultMigrationFileName
                                    : Constants.BaseFileName;
            }
            catch (Exception ex)
            {
                RedirectToErrorPage(ex.Message);
            }
        }

        private void WebAppSelectorOnContextChange(object sender, EventArgs e)
        {
            // user changed the web application selection
            // NOTE: this event also fire when the page first loads
            var webApplication = ((WebApplicationSelector)sender).CurrentItem;
            if (webApplication == null)
            {
                litWebAppName.Text = "n/a";
                return;
            }

            litWebAppName.Text = string.Format("{0}", string.Format("{0}", webApplication.Name));

            var sites = webApplication.Sites.Select(s => new ListItem(s.ServerRelativeUrl, s.Url)).ToArray();
            SourceSiteCollectionSelector.Items.AddRange(sites);
            SourceSiteCollectionSelector.SelectedValue = 
                ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ExportSection, Constants.SourceSiteElementName);
            DestinationSiteCollectionSelector.Items.AddRange(sites);
            SourceSiteCollectionSelector.DataBind();
            DestinationSiteCollectionSelector.SelectedValue = 
                ConfigurationHelper.GetMigrationConfigItem<string>(_configurationHelper.ImportSection, Constants.DestinationSiteElementName);
            DestinationSiteCollectionSelector.DataBind();
        }

        private void OnBtnCancelClick(object sender, EventArgs e)
        {
            RedirectOnOK();
        }

        private void OnBtnOkClick(object sender, EventArgs e)
        {
            try
            {
                GetConfiguration();
                SaveSettings();
                ////WebAppSelector.CurrentItem.JobDefinitions.Single(c => c.Name == Constants.MigrationContentJobName).RunNow();
                RedirectOnOK();
            }
            catch (Exception ex)
            {
                RedirectToErrorPage(ex.Message);
            }
        }

        private void SaveSettings()
        {
            ConfigurationHelper.SaveOrCreateElement(_configurationHelper.ImportSection, Constants.DestinationSiteElementName, DestinationSiteCollectionSelector.SelectedValue);
            ConfigurationHelper.SaveOrCreateElement(_configurationHelper.ExportSection, Constants.SourceSiteElementName, SourceSiteCollectionSelector.SelectedValue);
            ConfigurationHelper.SaveOrCreateElement(_configurationHelper.ExportSection, Constants.FileLocationElementName, FileLocation.Text);
            ConfigurationHelper.SaveOrCreateElement(_configurationHelper.ImportSection, Constants.FileLocationElementName, FileLocation.Text);
            ConfigurationHelper.SaveOrCreateElement(_configurationHelper.ExportSection, Constants.BaseFileNameElementName, FileName.Text);
            ConfigurationHelper.SaveOrCreateElement(_configurationHelper.ImportSection, Constants.BaseFileNameElementName, FileName.Text);
            ConfigurationHelper.SaveOrCreateElement(_configurationHelper.ExportSection, Constants.ExportMethodElementName, ExportTypes.SelectedValue);
            _configurationHelper.Save();
        }
    }
}
