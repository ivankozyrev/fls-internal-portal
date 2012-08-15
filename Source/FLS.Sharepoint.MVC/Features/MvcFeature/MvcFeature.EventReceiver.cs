using System;
using System.Linq;
using System.Runtime.InteropServices;
using FLS.SharePoint.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FLS.Sharepoint.MVC.Features.MvcFeature
{
    [Guid("1f054646-6dd6-4f03-9658-0c03787432a4")]
    public class MvcFeatureEventReceiver : SPFeatureReceiver
    {
        private const string SPWebConfigModificationOwner = "FLS";

        private static readonly IServiceLocator ServiceLocator = SharePointServiceLocator.GetCurrent();

        private static readonly ILogger Log = ServiceLocator.GetInstance<ILogger>();

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var app = properties.Feature.Parent as SPWebApplication;
            if (app == null)
            {
                return;
            }
            
            var name = "add[@name='MvcEngine']";
            var xpath = "configuration/system.webServer/handlers";
            var value = "<add name='MvcEngine' verb='*' path='mvcengine/*' type='FLS.Sharepoint.MVC.MvcHandler, FLS.Sharepoint.MVC, version=1.0.0.0, culture=neutral, publickeytoken=73a5aa681943dea6' resourceType='Unspecified' />";
            ModifyWebConfig(app, name, xpath, value, SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode);

            name = "system.codedom";
            xpath = "configuration";
            ModifyWebConfig(app, name, xpath, string.Empty, SPWebConfigModification.SPWebConfigModificationType.EnsureSection);

            name = "compilers";
            xpath = "configuration/system.codedom";
            ModifyWebConfig(app, name, xpath, string.Empty, SPWebConfigModification.SPWebConfigModificationType.EnsureSection);

            name = "compiler[@language='c#;cs;csharp']";
            xpath = "configuration/system.codedom/compilers";
            value = "<compiler language='c#;cs;csharp' extension='.cs' warningLevel='4' type='Microsoft.CSharp.CSharpCodeProvider, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'><providerOption name='CompilerVersion' value='v3.5'/><providerOption name='WarnAsError' value='false'/></compiler>";
            ModifyWebConfig(app, name, xpath, value, SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode);
            try
            {
                app.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
            }
            catch (Exception ex)
            {
                RemoveAllModifications(app);
                Log.Error(ex);
                throw;
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var app = properties.Feature.Parent as SPWebApplication;
            if (app == null)
            {
                return;
            }

            RemoveAllModifications(app);

            try
            {
                app.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        private static void ModifyWebConfig(SPWebApplication webApp, string name, string path, string value, SPWebConfigModification.SPWebConfigModificationType type)
        {
            if (AlreadyExists(webApp, name))
            {
                return;
            }

            var modification = new SPWebConfigModification(name, path)
                                   {
                                       Value = value, 
                                       Sequence = 0, 
                                       Type = type,
                                       Owner = SPWebConfigModificationOwner
                                   };

            try
            {
                webApp.WebConfigModifications.Add(modification);
                webApp.Update();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        private static void RemoveAllModifications(SPWebApplication application)
        {
            var modificationsToRemove = application.WebConfigModifications
                .Where(modification => modification.Owner == SPWebConfigModificationOwner)
                .ToList();

            foreach (var modification in modificationsToRemove)
            {
                application.WebConfigModifications.Remove(modification);
                application.Update();
            }
        }

        private static bool AlreadyExists(SPWebApplication application, string name)
        {
            return application.WebConfigModifications
                .Any(m => m.Owner == SPWebConfigModificationOwner && m.Name == name);
        }
    }
}
