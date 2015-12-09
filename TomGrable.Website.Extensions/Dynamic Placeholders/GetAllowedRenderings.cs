using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPlaceholderRenderings;

namespace TomGrable.Website.Extensions.DynamicPlaceholders
{
    /// <summary>
    ///  This GetPlaceholderRenderings pipeline processor replaces the stock Sitecore
    ///  GetAllowedRenderings processor with an implementation that takes dynamic
    ///  placeholder keys into account.
    /// </summary>
    public class GetAllowedRenderingsDynamic
    {
        private GetAllowedRenderings GetRenderingsProcessor = new GetAllowedRenderings();

        public void Process(GetPlaceholderRenderingsArgs args)
        {
            Assert.IsNotNull(args, "args");

            // If the placeholder key contains the dynamic suffix...
            if (args.PlaceholderKey.ToLower().Contains(Constants.DynamicPlaceholderSuffix.ToLower()))
            {
                // Strip off the suffix to get the actual placeholder key.
                var actualKey = args.PlaceholderKey.ToLower().Substring(0, args.PlaceholderKey.ToLower().LastIndexOf(Constants.DynamicPlaceholderSuffix.ToLower()));

                // Re-build the arguments using the actual placeholder key.
                var newArgs = args.DeviceId.IsNull
                                ? new GetPlaceholderRenderingsArgs(actualKey, args.LayoutDefinition, args.ContentDatabase)
                                : new GetPlaceholderRenderingsArgs(actualKey, args.LayoutDefinition, args.ContentDatabase, args.DeviceId);

                // Call Sitecore's processor using our new arguments.
                GetRenderingsProcessor.Process(newArgs);

                // Bring Sitecore's processor's results into our args variable.
                args.Options.ShowTree = newArgs.Options.ShowTree;
                args.Options.ShowRoot = newArgs.Options.ShowRoot;
                args.Options.SetRootAsSearchRoot = newArgs.Options.SetRootAsSearchRoot;
                args.HasPlaceholderSettings = newArgs.HasPlaceholderSettings;
                args.OmitNonEditableRenderings = newArgs.OmitNonEditableRenderings;
                args.PlaceholderRenderings = newArgs.PlaceholderRenderings;
            }
            else
            {
                // Otherwise, just fall back on Sitecore's implementations for non-dynamic placeholders.
                GetRenderingsProcessor.Process(args);
            }
        }
    }
}
