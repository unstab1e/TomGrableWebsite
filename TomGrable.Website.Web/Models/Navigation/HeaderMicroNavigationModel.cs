using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Synthesis;
using Synthesis.Model;

namespace TomGrable.Website.Web.Models.Navigation
{
    public class HeaderMicroNavigationModel
    { 
        private readonly List<INavigationBaseItem> _navigationBaseItems = new List<INavigationBaseItem>();

        public HeaderMicroNavigationModel(IFolderItem folder) {
            foreach (Item i in folder.Children)
            {
                var navItem = i.As<INavigationBaseItem>();
                if (null != navItem) {
                    _navigationBaseItems.Add(navItem);
                }
            }
        }

        public List<INavigationBaseItem> NavigationBaseItems
        {
            get { return _navigationBaseItems; }
        }
    }
}