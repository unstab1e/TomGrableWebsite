using System;
using System.Collections;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Configuration;
using Sitecore.Analytics.Data.Items;
using Sitecore.Analytics.Media;
//using Sitecore.Analytics.Tracking;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Sites;

namespace TomGrable.Website.Extensions.Events.MediaRequest
{
    public class MediaRequestEventHandler
    {
        private readonly ArrayList _trackedMediaFolders;

        public ArrayList TrackedMediaFolders
        {
            get
            {
                return _trackedMediaFolders;
            }
        }
        public MediaRequestEventHandler()
        {
            _trackedMediaFolders = new ArrayList();
        }

        public void OnMediaRequest(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            if (!AnalyticsSettings.Enabled)
                return;
            var site = Context.Site;
            if (site == null || !site.EnableAnalytics)
                return;
            var request =
                Event.ExtractParameter(args, 0) as Sitecore.Resources.Media.MediaRequest;
            if (request == null)
                return;
            var trackingInformation = new MediaRequestTrackingInformation(request);
            var mediaItem = trackingInformation.GetMediaItem();
            if (mediaItem == null)
                return;
            if (!IsTrackedForDownloads(mediaItem))
                return;
            Log.Info("Triggering download event", this);
            using (new ContextItemSwitcher(mediaItem))
            {
                try
                {
                    //if (!Tracker.IsActive)
                    //    return;
                    //var previousPage = Tracker.Current.Session.Interaction.PreviousPage;

                    //if (previousPage == null)
                    //    return;
                    //if (Tracker.Current.Session.Interaction.CurrentPage.PageEvents.All(pageEvent => pageEvent.PageEventDefinitionId != AnalyticsIds.DownloadEvent.Guid))
                    //    return;
                    //Log.Info("Download event is not defined on the page, triggering programatically", this);
                    //var item = mediaItem.Database.GetItem(AnalyticsIds.DownloadEvent);
                    //if (item == null)
                    //    return;
                    //var eventItem = new PageEventItem(item);
                    //previousPage.Register(eventItem);
                }
                catch (Exception ex)
                {
                    Log.Error("Media request analytics failed", ex, this.GetType());
                }
            }
            Log.Info("Triggered download event", this);

        }

        private bool IsTrackedForDownloads(Item mediaItem)
        {
            return TrackedMediaFolders.ToArray().Any(path => mediaItem.Paths.FullPath.Contains(path.ToString()));
        }
    }
}
