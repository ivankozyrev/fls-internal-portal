using System;
using System.Diagnostics;
using System.IO;
using Microsoft.SharePoint.Administration;

namespace FLS.SharePoint.System
{
    public class PackagesDeploymentJob : SPJobDefinition
    {
        private static readonly string sharedFolder = @"C:\SPPackages";

        private static readonly string stsadmPath = @"C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\BIN\STSADM.EXE";

        public PackagesDeploymentJob()
        {
        }

        public PackagesDeploymentJob(string name, SPService service, SPServer server, SPJobLockType lockType) : base(name, service, server, lockType)
        {
        }

        public PackagesDeploymentJob(string name, SPWebApplication webApplication) : base(name, webApplication, SPServer.Local, SPJobLockType.Job)
        {
            this.Title = "FLS - Packages Deployment Job";
        }

        public override void Execute(Guid targetInstanceId)
        {
            var packagesList = Directory.GetFiles(sharedFolder, "*.wsp");
            foreach (var packagePath in packagesList)
            {
                var process = new Process();
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.WorkingDirectory = sharedFolder;
                process.StartInfo.FileName = stsadmPath;
                process.StartInfo.CreateNoWindow = true;
                // process.StartInfo.Arguments = "-o deletesolution -name " + packagePath;
                process.StartInfo.Arguments = "-o addsolution -filename " + packagePath;
                process.Start();
                process.WaitForExit(5000);
                this.Title = process.StandardOutput.ReadToEnd();
                this.Update();
            }
        }
    }
}