using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Synthesis;
using Synthesis.Model;

namespace TomGrable.Website.Web.Models.Navigation
{
    public class NavigationModel {
        private readonly IEnumerable<IStandardTemplateItem> _navigationItems;

        public NavigationModel(IEnumerable<IStandardTemplateItem> navigationItems) {
            _navigationItems = navigationItems;
        }

        public IEnumerable<IStandardTemplateItem> NavigationItems {
            get {
                return _navigationItems;
            }
        }
    }

}