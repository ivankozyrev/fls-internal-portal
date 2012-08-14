using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;

namespace FLS.SharePoint.ListDefinition.Features.HRLibraryFeature
{
    [Guid("79f98817-fa5a-4b14-950f-c232c786e3a6")]
    public class HRLibraryFeatureEventReceiver : SPFeatureReceiver
    {
        private static readonly string[] ContentTypeNames = new[] { "ApplicationForAdmission", "ApplicationForUnpaidLeave" };

        private readonly Hashtable _contentTypeSet = new Hashtable();

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            if (site == null)
            {
                return;
            }

            CreateContentTypeSet(site);
            var documentsLibrary = site.RootWeb.Lists["HR Library"];
            if (documentsLibrary != null)
            {
                AssignContentTypesToLibrary(documentsLibrary);    
            }
        }

        private void AssignContentTypesToLibrary(SPList library)
        {
            foreach (var contentTypeName in ContentTypeNames.Where(contentTypeName => library.ContentTypes[contentTypeName] == null))
            {
                library.ContentTypes.Add(_contentTypeSet[contentTypeName] as SPContentType);
            }
        }

        private void CreateContentTypeSet(SPSite site)
        {
            foreach (var contentTypeName in ContentTypeNames)
            {
                _contentTypeSet.Add(contentTypeName, site.RootWeb.AvailableContentTypes[contentTypeName]);
            }
        }
    }
}
