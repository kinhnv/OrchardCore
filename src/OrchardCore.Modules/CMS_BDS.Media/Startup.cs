using System;
using System.IO;
using CMS_BDS.Media.Controllers;
using CMS_BDS.Media.Services;
using CMS_BDS.Media.ViewModels;
using Fluid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Environment.Shell;
using OrchardCore.FileStorage;
using OrchardCore.FileStorage.FileSystem;
using OrchardCore.Liquid;
using OrchardCore.Media;
using OrchardCore.Media.Core;
using OrchardCore.Media.Models;
using OrchardCore.Modules;
using OrchardCore.Modules.FileProviders;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using SixLabors.ImageSharp.Web.DependencyInjection;

namespace CMS_BDS.Media
{
    public class Startup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public Startup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }
        static Startup()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<DisplayMediaFieldViewModel>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<MediaOptions>, MediaOptionsConfiguration>();

            services.AddSingleton<IMediaFileProvider>(serviceProvider =>
            {
                var shellOptions = serviceProvider.GetRequiredService<IOptions<ShellOptions>>();
                var shellSettings = serviceProvider.GetRequiredService<ShellSettings>();
                var options = serviceProvider.GetRequiredService<IOptions<MediaOptions>>().Value;

                var mediaPath = GetMediaPath(shellOptions.Value, shellSettings, options.AssetsPath);

                if (!Directory.Exists(mediaPath))
                {
                    Directory.CreateDirectory(mediaPath);
                }
                return new MediaFileProvider(options.AssetsRequestPath, mediaPath);
            });

            services.AddSingleton<IStaticFileProvider, IMediaFileProvider>(serviceProvider =>
                serviceProvider.GetRequiredService<IMediaFileProvider>()
            );

            services.AddSingleton<IMediaFileStore>(serviceProvider =>
            {
                var shellOptions = serviceProvider.GetRequiredService<IOptions<ShellOptions>>();
                var shellSettings = serviceProvider.GetRequiredService<ShellSettings>();
                var mediaOptions = serviceProvider.GetRequiredService<IOptions<MediaOptions>>().Value;

                var mediaPath = GetMediaPath(shellOptions.Value, shellSettings, mediaOptions.AssetsPath);
                var fileStore = new FileSystemStore(mediaPath);

                var mediaUrlBase = "/" + fileStore.Combine(shellSettings.RequestUrlPrefix, mediaOptions.AssetsRequestPath);

                var originalPathBase = serviceProvider.GetRequiredService<IHttpContextAccessor>()
                    .HttpContext?.Features.Get<ShellContextFeature>()?.OriginalPathBase ?? null;

                if (originalPathBase.HasValue)
                {
                    mediaUrlBase = fileStore.Combine(originalPathBase.Value, mediaUrlBase);
                }

                return new DefaultMediaFileStore(fileStore, mediaUrlBase, mediaOptions.CdnBaseUrl);
            });

            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<IAuthorizationHandler, AttachedMediaFieldsFolderAuthorizationHandler>();
            services.AddScoped<INavigationProvider, AdminMenu>();

            services.AddMedia();

            // Media Field
            services.AddScoped<AttachedMediaFieldFileService, AttachedMediaFieldFileService>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {

            var mediaFileProvider = serviceProvider.GetRequiredService<IMediaFileProvider>();
            var mediaOptions = serviceProvider.GetRequiredService<IOptions<MediaOptions>>().Value;
            var mediaFileStoreCache = serviceProvider.GetService<IMediaFileStoreCache>();

            // FileStore middleware before ImageSharp, but only if a remote storage module has registered a cache provider.
            if (mediaFileStoreCache != null)
            {
                app.UseMiddleware<MediaFileStoreResolverMiddleware>();
            }

            // ImageSharp before the static file provider.
            app.UseImageSharp();

            // Use the same cache control header as ImageSharp does for resized images.
            var cacheControl = "public, must-revalidate, max-age=" + TimeSpan.FromDays(mediaOptions.MaxBrowserCacheDays).TotalSeconds.ToString();

            app.UseStaticFiles(new StaticFileOptions
            {
                // The tenant's prefix is already implied by the infrastructure.
                RequestPath = mediaOptions.AssetsRequestPath,
                FileProvider = mediaFileProvider,
                ServeUnknownFileTypes = true,
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = cacheControl;
                }
            });

            routes.MapAreaControllerRoute(
                name: "Media.Index",
                areaName: "CMS_BDS.Media",
                pattern: _adminOptions.AdminUrlPrefix + "/BDSMedia",
                defaults: new { controller = typeof(AdminController).ControllerName(), action = nameof(AdminController.Index) }
            );
        }

        private string GetMediaPath(ShellOptions shellOptions, ShellSettings shellSettings, string assetsPath)
        {
            return PathExtensions.Combine(shellOptions.ShellsApplicationDataPath, shellOptions.ShellsContainerName, shellSettings.Name, assetsPath);
        }
    }
}