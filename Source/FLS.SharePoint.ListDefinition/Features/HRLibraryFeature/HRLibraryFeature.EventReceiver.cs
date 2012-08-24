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

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            if (site == null)
            {
                return;
            }

            var contentTypeSet = CreateContentTypeSet(site); 
            var fieldSet = CreateFieldSet(site);
            
            var documentsLibrary = site.RootWeb.Lists["HR Library"];
            if (documentsLibrary == null)
            {
                return;
            }

            AssignContentTypesToLibrary(documentsLibrary, contentTypeSet);
            AssignFieldsToLibrary(documentsLibrary, fieldSet);
        }

        private static void AssignFieldsToLibrary(SPList library, Hashtable fieldSet)
        {
            foreach (var field in FieldNames.Where(fieldName => !library.Fields.ContainsField(fieldName)).Select(fieldName => fieldSet[fieldName] as SPField))
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

        private static void AssignContentTypesToLibrary(SPList library, Hashtable contentTypeSet)
        {
            foreach (var contentType in ContentTypeNames.Where(contentTypeName => library.ContentTypes[contentTypeName] == null).Select(contentTypeName => contentTypeSet[contentTypeName] as SPContentType))
            {
                contentType.DocumentTemplate = library.ParentWebUrl + contentType.DocumentTemplate;
                library.ContentTypes.Add(contentType);
            }
        }

        private static Hashtable CreateContentTypeSet(SPSite site)
        {
            var result = new Hashtable();
            foreach (var contentTypeName in ContentTypeNames.Where(contentTypeName => !result.ContainsKey(contentTypeName)))
            {
                result.Add(contentTypeName, site.RootWeb.AvailableContentTypes[contentTypeName]);
            }

            return result;
        }

        private static Hashtable CreateFieldSet(SPSite site)
        {
            var result = new Hashtable();
            foreach (var fieldName in FieldNames.Where(fieldName => !result.ContainsKey(fieldName)))
            {
                result.Add(fieldName, site.RootWeb.AvailableFields[fieldName]);
            }

            return result;
        }
    }
}
