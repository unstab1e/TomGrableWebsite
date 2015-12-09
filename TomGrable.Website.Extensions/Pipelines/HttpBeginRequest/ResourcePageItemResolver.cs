using System;
using System.Linq;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Pipelines.HttpRequest;
using Synthesis.Model;

namespace TomGrable.Website.Extensions.Pipelines.HttpBeginRequest
{
    /// <summary>
    /// This item resolver ensures that resource URLs work whenever they do not include bucket folders.
    /// (e.g. /resources/universal-ink/resource-page vs. /resources/universal-ink/2015/07/22/resource-page)
    /// </summary>
    public class ResourcePageItemResolver : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            // If an item has already been resolved, we do nothing.
            if (Context.Item != null) return;

            // Pop the last part of the item path off to get the parent item's path.
            var index = args.Url.ItemPath.LastIndexOf('/'); if (index <= 0) return;
            var parentPath = args.Url.ItemPath.Substring(0, index);

            // Decode the path to the parent item (translate '-' to ' ' and such).
            parentPath = MainUtil.DecodeName(parentPath);

            // Get the parent item.
            var parentItem = args.GetItem(parentPath);
            if (parentItem == null) return;

            //// If the parent item isn't a resource folder then we do nothing.
            //if (parentItem.TemplateID != ResourcesFolder.ItemTemplateId) return;

            // Get the name of the requested item from the original path.
            var itemName = args.Url.ItemPath.Substring(index + 1);

            // Need to decode the item name too.
            itemName = MainUtil.DecodeName(itemName);

            // Find an item in the resource folder with a matching item name.
            using (var context = ContentSearchManager.GetIndex(parentItem as IIndexable).CreateSearchContext())
            {
                var result = context.GetQueryable<SearchResultItem>().Where(i => i.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (result != null) Context.Item = result.GetItem();
            }
        }
    }
}
