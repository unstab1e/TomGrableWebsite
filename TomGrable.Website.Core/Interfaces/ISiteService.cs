using System.Collections.Generic;
using Synthesis.Model;

namespace TomGrable.Website.Core.Interfaces
{
    public interface ISiteService
    {
        /// <summary>
        /// gets the SiteSettingsItem, as configued in site definition
        /// </summary>
        /// <returns></returns>
        //ISiteSettingsItem GetSettingsItem();

        /// <summary>
        /// gets the cookie policy text from SiteSettingsItem
        /// </summary>
        /// <returns></returns>
        //string GetSiteCookiePolicyNotice();

        /// <summary>
        /// gets a list of countries from culture info, the list is cached for 4 hours
        /// </summary>
        /// <returns></returns>
        //SortedDictionary<string, string> GetCountries();

        string RemoveSpecialCharacters(string input);
    }
}
