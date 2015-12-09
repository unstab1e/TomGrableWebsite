using System;
using System.Collections.Generic;
using System.Web;

using Sitecore.Configuration;
using Sitecore.Mvc.Helpers;
using Sitecore.StringExtensions;
using TomGrable.Website.Extensions.DynamicPlaceholders;

namespace TomGrable.Website.Extensions
{
    public static class HelperExtension
    {
        /// <summary>
        /// Cache the list of placeholders for the current request so that we can add to it as
        /// placeholders on the page are processed.
        /// </summary>
        private static List<string> DynamicPlaceholderList
        {
            get
            {
                // Get the placeholder list for the current request.
                var placeholders = HttpContext.Current.Items["DynamicPlaceholders"];

                // If no list has been created yet, create one.
                if (placeholders == null)
                    placeholders = HttpContext.Current.Items["DynamicPlaceholders"] = new List<string>();

                // Return the list.
                return (List<string>)placeholders;
            }

            set { HttpContext.Current.Items["DynamicPlaceholders"] = value; }
        }

        /// <summary>
        /// Sitecore HTML helper that passes a dynamic placeholder key to Sitecore's placeholder implementation.
        /// </summary>
        public static HtmlString DynamicPlaceholder(this SitecoreHelper helper, string placeholderName)
        {
            bool foundOne = false;
            int count = 0;

            // Dynamic placeholder names start with this.
            var dynamicNamePrefix = "{0}{1}".FormatWith(placeholderName, Constants.DynamicPlaceholderSuffix);

            // Loop through the placeholders that have already been processed and count
            // how many times this placeholder has already occurred.
            foreach (var placeholder in DynamicPlaceholderList)
            {
                if (placeholder == placeholderName || placeholder.StartsWith(dynamicNamePrefix))
                {
                    foundOne = true;
                    count++;
                }
            }

            // If any existing ones were found, tack the count onto the end to make this one unique.
            // (otherwise, we can use the placeholder name without any suffix).
            if (foundOne) placeholderName = "{0}{1}_{2}".FormatWith(placeholderName, Constants.DynamicPlaceholderSuffix, count);

            // Add the new placeholder name to the list.
            DynamicPlaceholderList.Add(placeholderName);

            // Pass the new placeholder name on to Sitecore's implementation.
            return helper.Placeholder(placeholderName);
        }
    }
}
