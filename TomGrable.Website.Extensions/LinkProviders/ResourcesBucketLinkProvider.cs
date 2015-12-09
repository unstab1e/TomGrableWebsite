using System.IO;
using Sitecore.Buckets.Extensions;
using Sitecore.Buckets.Managers;
using Sitecore.Data.Items;
using Sitecore.IO;
using Sitecore.Links;
using Synthesis.Model;

namespace TomGrable.Website.Extensions.LinkProviders
{
    public class ResourcesBucketLinkProvider : LinkProvider
    {
        /// <summary>
        /// This link provider ensures URLs for resources stored in a resources folder do not
        /// include all of the bucket folders (e.g. /resources/universal-ink/resource-page
        /// vs. /resources/universal-ink/2015/07/22/resource-page).
        /// </summary>
        public override string GetItemUrl(Item item, UrlOptions options)
        {
            // We're only concerned with items that are in a bucket.
            if (!BucketManager.IsItemContainedWithinBucket(item)) return base.GetItemUrl(item, options);

            // Get the bucket.  We're only interested in ResourcesFolder buckets here.
            var bucketItem = item.GetParentBucketItemOrParent();
            if (bucketItem == null || !bucketItem.IsABucket())/// || bucketItem.TemplateID != ResourcesFolder.ItemTemplateId)
                return base.GetItemUrl(item, options);

            // Get the url of the bucket itself.  Strip off .aspx if enabled.
            var bucketUrl = base.GetItemUrl(bucketItem, options);
            if (options.AddAspxExtension) bucketUrl = bucketUrl.Replace(".aspx", string.Empty);

            // Get the url of the item from the base implementation.
            var itemUrl = base.GetItemUrl(item, options);

            // Tack the original url's filename onto the bucket's URL to get the new item URL.
            itemUrl = FileUtil.MakePath(bucketUrl, Path.GetFileName(itemUrl));

            // Return the item's URL.
            return itemUrl;
        }
    }
}
