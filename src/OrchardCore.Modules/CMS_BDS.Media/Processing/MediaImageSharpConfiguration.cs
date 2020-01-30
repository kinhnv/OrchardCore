using Microsoft.Extensions.Options;
using OrchardCore.Media;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Processors;

namespace CMS_BDS.Media.Processing
{
    /// <summary>
    /// Provides default configuration for ImageSharp.
    /// </summary>
    public class MediaImageSharpConfiguration : IConfigureOptions<ImageSharpMiddlewareOptions>
    {
        private readonly MediaOptions _mediaOptions;

        public MediaImageSharpConfiguration(IOptions<MediaOptions> mediaOptions)
        {
            _mediaOptions = mediaOptions.Value;
        }

        public void Configure(ImageSharpMiddlewareOptions options)
        {
            options.Configuration = Configuration.Default;
            options.MaxBrowserCacheDays = _mediaOptions.MaxBrowserCacheDays;
            options.MaxCacheDays = _mediaOptions.MaxCacheDays;
            options.CachedNameLength = 12;
            options.OnParseCommands = validation =>
            {
                // Force some parameters to prevent disk filling.
                // For more advanced resize parameters the usage of profiles will be necessary.
                // This can be done with a custom IImageWebProcessor implementation that would
                // accept profile names.

                if (validation.Commands.Count > 0)
                {
                    validation.Commands.Remove(ResizeWebProcessor.Compand);
                    validation.Commands.Remove(ResizeWebProcessor.Sampler);
                    validation.Commands.Remove(ResizeWebProcessor.Xy);
                    validation.Commands.Remove(ResizeWebProcessor.Anchor);
                    validation.Commands.Remove(BackgroundColorWebProcessor.Color);

                    if (!validation.Commands.ContainsKey(ResizeWebProcessor.Mode))
                    {
                        validation.Commands[ResizeWebProcessor.Mode] = "max";
                    }
                }
            };
            options.OnProcessed = _ => { };
            options.OnPrepareResponse = _ => { };
        }
    }
}
