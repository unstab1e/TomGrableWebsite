using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;

namespace TomGrable.Website.Extensions.DynamicPlaceholders
{
    /// <summary>
    /// This GetChromeData pipeline processor runs after Sitecore's processor for
    /// placeholder chrome data and makes the placeholder key appear using the actual name
    /// instead of the dynamic name in the Experience Editor.
    /// </summary>
    public class GetChromeData : GetChromeDataProcessor
    {
        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.ChromeData, "Chrome Data");

            // Can't process chrome for a null item.
            if (args.Item == null) return;

            // If this request is for placeholder chrome...
            if (args.ChromeType.Equals("placeholder", StringComparison.OrdinalIgnoreCase))
            {
                // Get the placeholder key.
                var placeholderKey = args.CustomData["placeHolderKey"] as string;

                // If this placeholder is not a dynamic placeholder, do nothing.
                if (!placeholderKey.ToLower().Contains(Constants.DynamicPlaceholderSuffix.ToLower()))
                    return;

                // Strip off the suffix to get the actual placeholder key.
                var actualKey = placeholderKey.ToLower().Substring(0, placeholderKey.ToLower().LastIndexOf(Constants.DynamicPlaceholderSuffix.ToLower()));

                // Get the current item's layout definition.
                var layout = args.Item[Sitecore.FieldIDs.LayoutField];

                // Get the placeholder item for this placeholder on the current item's layout definition.
                var placeholderItem = Sitecore.Client.Page.GetPlaceholderItem(placeholderKey, args.Item.Database, layout);
                if (placeholderItem == null) return;

                // Use the placeholder's display name as the display name in the chrome data.
                args.ChromeData.DisplayName = placeholderItem.DisplayName;

                // If the placeholder has a short description, use that in the chrome data's expanded display name.
                if (!string.IsNullOrWhiteSpace(placeholderItem.Appearance.ShortDescription))
                    args.ChromeData.ExpandedDisplayName = placeholderItem.Appearance.ShortDescription;
            }
        }
    }
}
