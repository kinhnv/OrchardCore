using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using CMS_BDS.Media.Fields;
using CMS_BDS.Media.Settings;

namespace CMS_BDS.Media
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        // This migration does not need to run on new installations, but because there is no
        // initial migration record, there is no way to shortcut the Create migration.
        public int Create()
        {
            _contentDefinitionManager.MigrateFieldSettings<MediaField, MediaFieldSettings>();

            return 1;
        }
    }
}
