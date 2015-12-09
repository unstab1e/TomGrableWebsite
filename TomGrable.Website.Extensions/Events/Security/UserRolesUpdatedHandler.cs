using System;
//using System.Diagnostics;
//using System.Linq;
//using System.Web.Management;
//using Coveo.Framework.Utils;
//using Sitecore.ContentSearch.Linq.Extensions;
using Sitecore.Diagnostics;
//using Sitecore.Eventing.Remote;
using Sitecore.Events;
using Sitecore.StringExtensions;
//using TomGrable.Website.Data;

namespace TomGrable.Website.Extensions.Events.Security
{
    /// <summary>
    /// Event handler that us used when a user is added to role(s) or is removed from role(s)
    /// </summary>
    public class UserRolesUpdatedHandler
    {
        //public void Process(object sender, EventArgs args)
        //{
        //    Assert.ArgumentNotNull(sender, "sender");
        //    Assert.ArgumentNotNull(args, "args");
        //    var scArgs = (SitecoreEventArgs)args;
        //    Assert.IsNotNull(scArgs, "scArgs != null");
        //    var usernames = Event.ExtractParameter(scArgs, 0) as string[];
        //    var roleNames = Event.ExtractParameter(scArgs, 1) as string[];
        //    Assert.IsNotNull(roleNames, "roleNames != null");
        //    Assert.IsNotNull(usernames, "usernames != null");

        //    try
        //    {
        //        var eventTime = DateTime.UtcNow;
        //        using (var context = new WacomAppDataContext())
        //        {
        //            var eventData = "Event Name: {0}, usernames: {1}, roles: {2}".FormatWith(scArgs.EventName, string.Join(" ,", usernames), string.Join(", ", roleNames));
        //            var queueItem =
        //                new EventQueue() { Created = eventTime, EventData = eventData };
        //            Log.Info("Adding event data to event queue {0}".FormatWith(eventData), this);
        //            lock (context.EventQueues)
        //            {
        //                context.EventQueues.InsertOnSubmit(queueItem);
        //                context.SubmitChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("error reading App database", ex, this);
        //    }
        //}
    }
}
