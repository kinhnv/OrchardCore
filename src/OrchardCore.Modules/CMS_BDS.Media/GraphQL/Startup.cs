using CMS_BDS.Media.Fields;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Apis;
using OrchardCore.Apis.GraphQL;
using OrchardCore.Modules;

namespace CMS_BDS.Media.GraphQL
{
    [RequireFeatures("OrchardCore.Apis.GraphQL")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISchemaBuilder, MediaAssetQuery>();
            services.AddObjectGraphType<MediaField, MediaFieldQueryObjectType>();
            services.AddTransient<MediaAssetObjectType>();
        }
    }
}
