using System.Threading.Tasks;
using System.Xml.Linq;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Sitemaps.Builders;
using OrchardCore.Sitemaps.Models;

namespace OrchardCore.Sitemaps.Sitemaps
{
    public class SitemapIndexBuilder : SitemapBuilderBase<SitemapIndex>
    {
        public override Task<XDocument> BuildSitemapsAsync(SitemapIndex sitemapIndex, SitemapBuilderContext context)
        {
            // Recurse ChildNodes, but only to one level. Nothing under an index can have multi-levels
            XNamespace defaultNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var root = new XElement(defaultNamespace + "sitemapindex");

            //TODO

            //foreach (var node in sitemapNode.ChildNodes)
            //{
            //    var sitemap = new XElement(defaultNamespace + "sitemap");
            //    var loc = new XElement(defaultNamespace + "loc");
            //    loc.Add(context.HostPrefix + context.UrlHelper.ActionContext.HttpContext.Request.PathBase + "/" + node.Path);
            //    sitemap.Add(loc);

            //    var lastModDate = await context.Builder.GetLastModifiedDateAsync(node, context);
            //    var lastMod = new XElement(defaultNamespace + "lastmod");
            //    lastMod.Add(lastModDate.GetValueOrDefault().ToString("yyyy-MM-ddTHH:mm:sszzz"));
            //    sitemap.Add(lastMod);

            //    root.Add(sitemap);
            //}
            var document = new XDocument(root);
            return Task.FromResult(new XDocument(document));
        }
    }
}
