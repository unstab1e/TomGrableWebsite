
using System.ComponentModel.DataAnnotations;
using Sitecore.Globalization;

namespace TomGrable.Website.Extensions.MvcExtensions.ComponentModel
{
    public class SitecoreRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return Translate.Text(ErrorMessageString, name);
        }
    }
}
