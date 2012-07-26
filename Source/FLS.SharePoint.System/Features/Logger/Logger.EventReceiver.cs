using System.Runtime.InteropServices;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace FLS.SharePoint.System.Features.Logger
{
    [Guid("5adc639f-148a-44fe-9fe7-3ab16fcaa8ab")]
    public class LoggerEventReceiver : SPFeatureReceiver
    {
        // This helper property builds a collection of areas and categories.
        private DiagnosticsAreaCollection areas;

        private DiagnosticsAreaCollection Areas
        {
            get
            {
                if (areas == null)
                {
                    areas = new DiagnosticsAreaCollection();
                    var area = new DiagnosticsArea("FLS.SharePoint");

                    area.DiagnosticsCategories.Add(
                        new DiagnosticsCategory(
                            "Debug", 
                            EventSeverity.Information, 
                            TraceSeverity.Medium));

                    areas.Add(area);
                }

                return areas;
            }
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var configManager = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();

            var configuredAreas = new DiagnosticsAreaCollection(configManager);
             
            foreach (var newArea in Areas)
            {
                var existingArea = configuredAreas[newArea.Name];

                if (existingArea == null)
                {
                    configuredAreas.Add(newArea);
                }
                else
                {
                    foreach (var category in newArea.DiagnosticsCategories)
                    {
                        var existingCategory = existingArea.DiagnosticsCategories[category.Name];
                        if (existingCategory == null)
                        {
                            existingArea.DiagnosticsCategories.Add(category);
                        }
                    }
                }
            }

            configuredAreas.SaveConfiguration();
            DiagnosticsAreaEventSource.EnsureConfiguredAreasRegistered();
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var configManager = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();
            var configuredAreas = new DiagnosticsAreaCollection(configManager);

            foreach (var area in Areas)
            {
                var areaToRemove = configuredAreas[area.Name];

                if (areaToRemove != null)
                {
                    foreach (var c in area.DiagnosticsCategories)
                    {
                        var existingCat = areaToRemove.DiagnosticsCategories[c.Name];
                        if (existingCat != null)
                        {
                            areaToRemove.DiagnosticsCategories.Remove(existingCat);
                        }
                    }

                    if (areaToRemove.DiagnosticsCategories.Count == 0)
                    {
                        configuredAreas.Remove(areaToRemove);
                    }
                }
            }

            configuredAreas.SaveConfiguration();
        }
    }
}
