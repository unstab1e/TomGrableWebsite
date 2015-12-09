using System;
using Sitecore.Buckets.Extensions;
using Sitecore.Buckets.Managers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;

namespace TomGrable.Website.Extensions.Events.ItemSaved
{
    public class ItemSavedHandler
    {
        public void Process(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var savedItem = Event.ExtractParameter(args, 0) as Item;
            Assert.IsNotNull(savedItem, "saved item can not be null");
            if (savedItem == null)
            {
                Log.Error("error creating item", this);
                return;
            }
            if (!savedItem.Database.Name.Equals("master", StringComparison.InvariantCultureIgnoreCase))
                return;
            if (!BucketManager.IsItemContainedWithinBucket(savedItem))
            {
                Log.Debug("Item {0}  is not contained in a bucket", savedItem);
                return;
            }
            var bucketItem = savedItem.GetParentBucketItemOrParent();
            if (!BucketManager.IsBucket(bucketItem))
                return;

            BucketManager.MoveItemIntoBucket(savedItem, bucketItem);

        }
    }
}
