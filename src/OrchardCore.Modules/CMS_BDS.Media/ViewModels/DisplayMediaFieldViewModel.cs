using CMS_BDS.Media.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace CMS_BDS.Media.ViewModels
{
    public class DisplayMediaFieldViewModel
    {
        public string[] Paths => Field.Paths;
        public MediaField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }
    }
}
