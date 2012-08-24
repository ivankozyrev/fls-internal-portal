using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

namespace FLS.SharePoint.ContentDeployment
{
    public partial class EditJobCustomLayout : WebPartPage
    {


        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            MasterPageFile = SPContext.Current.Web.MasterUrl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnRun.ServerClick += OnBtnRunClick;
            btnSave.ServerClick += OnBtnSaveClick;
            btnCancel.ServerClick += OnBtnCancelClick;
        }

        private void OnBtnRunClick(object sender, EventArgs e)
        {
            var exportJobName = "ExportContenJob";
            var importJobName = "ImportContenJob";
            
            var site = SPContext.Current.Site;
            site.AllowUnsafeUpdates = true;
            SPContext.Current.Web.AllowUnsafeUpdates = true;

            ExecuteJob(exportJobName, site);
            ExecuteJob(importJobName, site);
        }

        private void OnBtnSaveClick(object sender, EventArgs e)
        {

        }

        private void OnBtnCancelClick(object sender, EventArgs e)
        {

        }

        private void ExecuteJob(string jobName, SPSite site)
        {
            foreach (var job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == jobName)
                {
                    try
                    {
                        job.Execute(Guid.Empty);
                        return;
                    }
                    catch (Exception exception)
                    {
                        // todo:
                        throw;
                    }

                }
            }
        }

    }
}
