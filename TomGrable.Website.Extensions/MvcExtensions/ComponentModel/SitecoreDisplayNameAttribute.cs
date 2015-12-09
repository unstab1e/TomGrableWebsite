using System.ComponentModel;
using Sitecore.Diagnostics;
using Sitecore.Globalization;

namespace TomGrable.Website.Extensions.MvcExtensions.ComponentModel
{
    public class SitecoreDisplayNameAttribute : DisplayNameAttribute
    {

        public string Key { get; set; }

        public override string DisplayName
        {
            get
            {
                Assert.ArgumentNotNullOrEmpty(this.Key, "Key");
                return Translate.Text(this.Key);

            }
        }
    }
}
