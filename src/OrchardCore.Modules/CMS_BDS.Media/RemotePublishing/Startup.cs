using OrchardCore.Modules;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.MetaWeblog;

namespace CMS_BDS.Media.RemotePublishing
{
    [RequireFeatures("OrchardCore.RemotePublishing")]
    public class RemotePublishingStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMetaWeblogDriver, MediaMetaWeblogDriver>();
        }
    }
}
