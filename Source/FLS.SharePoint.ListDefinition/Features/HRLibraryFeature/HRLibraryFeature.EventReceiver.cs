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

        private static readonly string[] FieldNames = new[] { "DocumentStatus", "ConsolidatedComment" };

        private readonly Hashtable _contentTypeSet = new Hashtable();

        private readonly Hashtable _fieldSet = new Hashtable();

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            if (site == null)
            {
                return;
            }

            CreateFieldSet(site);
            CreateContentTypeSet(site);

            var documentsLibrary = site.RootWeb.Lists["HR Library"];
            if (documentsLibrary == null)
            {
                return;
            }

            AssignContentTypesToLibrary(documentsLibrary);
            AssignFieldsToLibrary(documentsLibrary);
        }

        private void AssignFieldsToLibrary(SPList library)
        {
            foreach (var field in FieldNames.Where(fieldName => !library.Fields.ContainsField(fieldName)).Select(fieldName => _fieldSet[fieldName] as SPField))
            {
                library.Fields.Add(field);

                // Doesn't work for some obscure reason
                // library.DefaultView.ViewFields.Add(field)
                // library.DefaultView.Update()
                var defaultView = library.DefaultView;
                defaultView.ViewFields.Add(field);
                defaultView.Update();
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

        private void CreateFieldSet(SPSite site)
        {
            foreach (var fieldName in FieldNames)
            {
                _fieldSet.Add(fieldName, site.RootWeb.AvailableFields[fieldName]);
            }
        }
    }
}
