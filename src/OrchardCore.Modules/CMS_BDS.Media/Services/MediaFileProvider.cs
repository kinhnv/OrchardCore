using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using OrchardCore.Media;

namespace CMS_BDS.Media.Services
{
    public class MediaFileProvider : PhysicalFileProvider, IMediaFileProvider
    {
        public MediaFileProvider(PathString virtualPathBase, string root) : base(root)
        {
            VirtualPathBase = virtualPathBase;
        }

        public MediaFileProvider(PathString virtualPathBase, string root, ExclusionFilters filters) : base(root, filters)
        {
            VirtualPathBase = virtualPathBase;
        }

        public PathString VirtualPathBase { get; }
    }
}
