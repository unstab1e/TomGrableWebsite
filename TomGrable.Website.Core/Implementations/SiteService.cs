using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.StringExtensions;
using Synthesis;
//using Wacom.DeveloperRelations.Core.Helpers;
using TomGrable.Website.Core.Interfaces;
using Synthesis.Model;
//using Constants = Wacom.DeveloperRelations.Core.Helpers.Constants;

namespace TomGrable.Website.Core.Implementations
{
    public class SiteService : ISiteService
    {
        //public ISiteSettingsItem GetSettingsItem()
        //{
        //    try
        //    {
        //        return CacheUtil.GetObject<ISiteSettingsItem>(Constants.CacheKeys.SiteSettingsItemKey.FormatWith(Context.GetSiteName()), () =>
        //        {
        //            var itemId = Context.Site.Properties[Constants.SiteKeys.SiteSettingsItemIdKey];
        //            if (string.IsNullOrEmpty(itemId)) return null;
        //            var item = Context.Database.GetItem(itemId).As<IStandardTemplateItem>();
        //            if (item is ISiteSettingsItem)
        //                return item.InnerItem.As<ISiteSettingsItem>();
        //            return null;
        //        }, TimeSpan.FromHours(4));


        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("site settings item is not defined", ex, this);
        //    }
        //    return null;
        //}

        //public string GetSiteCookiePolicyNotice()
        //{
        //    return GetSettingsItem().CookiePolicyNotice.RenderedValue;
        //}

        //public SortedDictionary<string, string> GetCountries() {
        //    return CacheUtil.GetObject(Constants.CacheKeys.CountryList, () => {
        //        var countries = new SortedDictionary<string, string>();
        //        var cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures).ToList();
        //        cultureInfos.ForEach(cultureInfo => {
        //            var regionInfo = new RegionInfo(cultureInfo.LCID);
        //            if (!countries.ContainsKey(regionInfo.EnglishName)) {
        //                countries.Add(regionInfo.EnglishName, regionInfo.Name);
        //            }
        //        });
        //        return countries;
        //    }, TimeSpan.FromHours(4));
        //}


        public string RemoveSpecialCharacters(string input)
        {
            var r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }
    }


}
