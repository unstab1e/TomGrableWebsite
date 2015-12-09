using System;
using System.Text.RegularExpressions;
using System.Web;

using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;

[assembly: PreApplicationStartMethod(typeof(TomGrable.Website.Extensions.ItemNamesWithSpaces.AddAndRenameHandler), "Start")]

namespace TomGrable.Website.Extensions.ItemNamesWithSpaces
{
    /// Sitecore item:added and item:renamed event handler that ensures
    /// item names are created with dashes instead of spaces and that
    /// the original name (with spaces) is stored in Display Name.
    public class AddAndRenameHandler
    {
        /// <summary>
        /// Start method that registers the Sitecore event handlers.
        /// </summary>
        public static void Start()
        {
            var handler = new AddAndRenameHandler();
            Event.Subscribe("item:added", new EventHandler(handler.OnEvent));
            Event.Subscribe("item:renamed", new EventHandler(handler.OnEvent));
        }

        /// <summary>
        /// Called when a Sitecore item is saved or renamed.
        /// </summary>
        public void OnEvent(object sender, EventArgs args)
        {
            try
            {
                // Pull the item from the args.
                var item = (Item)Event.ExtractParameter(args, 0);

                // Do nothing if the item isn't in the master database.
                if (item.Database.Name != "master") return;

                // We're only concerned with items under /sitecore/content
                // (so not Layouts or Templates or whatever).
                if (!item.Paths.Path.StartsWith("/sitecore/content")) return;

                // If spaces are found in the name, replace them.
                string newName;
                if (item.Name == (newName = Regex.Replace(item.Name.ToLower().Replace(' ', '-'), "-{2,}", "-")))
                {
                    // If no spaces were found, do nothing.
                    return;
                }

                // Edit the item that was just saved.
                using (new Sitecore.Data.Items.EditContext(item))
                {
                    // Set the display name to the original name (with spaces).
                    item.Appearance.DisplayName = item.Name;

                    // Set the item name to the new name (with dashes).
                    item.Name = newName;
                }
            }
            catch (Exception x)
            {
                Log.Error("An exception occurred trying to process an item in the AddAndRenameHandler.", x, this);
            }
        }
    }
}
