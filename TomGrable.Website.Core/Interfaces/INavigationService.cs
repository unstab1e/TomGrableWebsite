using System.Collections.Generic;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Synthesis;
using Synthesis.FieldTypes.Interfaces;
using Synthesis.Model;

namespace TomGrable.Website.Core.Interfaces
{
    public interface INavigationService
    {
        /// <summary>
        /// gets copyright text from SiteSettingsItem
        /// </summary>
        /// <returns></returns>
        //string GetCopyrightText();

        /// <summary>
        /// Gets the header background style from the current base page item.
        /// </summary>
        /// <returns>string</returns>
        //string GetHeaderBackgroundStyle();

        /// <summary>
        /// gets navigation items from datasource
        /// </summary>
        /// <returns></returns>
        IEnumerable<IStandardTemplateItem> GetNavigationItems();

        ///// <summary>
        ///// Returns a list of pages to include in the specified page's breadcrumb.
        ///// </summary>
        //IEnumerable<IBasePageItem> GetBreadcrumbForPage(IBasePageItem page);

        /// <summary>
        /// gets url from a synthesis link field
        /// </summary>
        /// <param name="linkField"></param>
        /// <returns></returns>
        string GetUrlFromLinkField(IHyperlinkField linkField);

        /// <summary>
        /// gets url from a sitecore link field
        /// </summary>
        /// <param name="linkField"></param>
        /// <returns></returns>
        string GetUrlFromLinkField(LinkField linkField);

        /// <summary>
        /// gets next sibling item of a given item, works with items in a bucket as well
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IStandardTemplateItem GetNextSibling(Item item);
        /// <summary>
        /// gets next sibling item of a given item, works with items in a bucket as well
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IStandardTemplateItem GetNextSibling(IStandardTemplateItem item);
        /// <summary>
        /// gets previous sibling item of a given item, works with items in a bucket as well
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IStandardTemplateItem GetPreviousSibling(Item item);
        /// <summary>
        /// gets previous sibling item of a given item, works with items in a bucket as well
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IStandardTemplateItem GetPreviousSibling(IStandardTemplateItem item);


    }
}
