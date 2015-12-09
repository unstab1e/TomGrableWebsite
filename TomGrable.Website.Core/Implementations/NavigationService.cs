using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Buckets.Extensions;
using Sitecore.Buckets.Managers;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using Sitecore.SecurityModel;
using Synthesis;
using Synthesis.FieldTypes.Interfaces;
using Synthesis.Utility;
using TomGrable.Website.Core.Interfaces;
using Constants = Sitecore.Buckets.Util.Constants;

namespace TomGrable.Website.Core.Implementations
{
    public class NavigationService : INavigationService
    {
        private readonly ISiteService _siteService;

        public NavigationService(ISiteService siteService)
        {
            _siteService = siteService;
        }

        //public string GetCopyrightText()
        //{
        //    var siteSettingsItem = _siteService.GetSettingsItem();
        //    return siteSettingsItem == null ? string.Empty : siteSettingsItem.Copyright.RenderedValue;
        //}

        //public string GetHeaderBackgroundStyle() {
        //    var item = Context.Item.As<IStandardTemplateItem>();
        //    if (!(item is IBasePageItem))
        //        return string.Empty;
        //    var basePage = Context.Item.As<IBasePageItem>();
        //    if (basePage.BackgroundStyle.HasValue && basePage.BackgroundStyle.Target is ILookupValueItem) {
        //        var lookupItem = basePage.BackgroundStyle.Target.InnerItem.As<ILookupValueItem>();
        //        return lookupItem.Title.RawValue;
        //    }
        //    return string.Empty;
        //}

        public IEnumerable<IStandardTemplateItem> GetNavigationItems()
        {
            if (RenderingContext.CurrentOrNull == null || RenderingContext.Current.Rendering.Item == null)
                return Enumerable.Empty<IStandardTemplateItem>(); ;
            var navigationRootItem = RenderingContext.Current.Rendering.Item.As<IStandardTemplateItem>();
            return navigationRootItem == null ? Enumerable.Empty<IStandardTemplateItem>() : navigationRootItem.Children;
        }

        /// <summary>
        /// Returns a list of pages to include in the specified page's breadcrumb.
        /// </summary>
        //public IEnumerable<IBasePageItem> GetBreadcrumbForPage(IBasePageItem page)
        //{
        //    // Create the collection.
        //    var pages = new List<IBasePageItem>();

        //    // The current page is always included unless it has been set not to.
        //    if (page.DisplayPageinBreadcrumb.Value)
        //        pages.Add(page);

        //    // Loop through the current page's ancestors - in reverse.
        //    foreach (var ancestor in page.Axes.GetAncestors().Reverse())
        //    {
        //        // If we reach the site root, stop.
        //        if (ancestor.TemplateId == WebsiteFolder.ItemTemplateId) break;

        //        // If the ancestor isn't a page, skip it.
        //        var ancestorPage = ancestor as IBasePageItem;
        //        if (ancestorPage == null) continue;

        //        // If the ancestor is supposed to be displayed in the breadcrumb, add it to the list.
        //        if (ancestorPage.DisplayPageinBreadcrumb.Value)
        //            pages.Add(ancestorPage);
        //    }

        //    // Reverse the list of pages so that the current page is last.
        //    pages.Reverse();

        //    // Return.
        //    return pages;
        //}

        public string GetUrlFromLinkField(IHyperlinkField linkField)
        {
            return linkField == null ? string.Empty : GetUrlFromLinkField(linkField.ToLinkField());
        }

        public string GetUrlFromLinkField(LinkField linkField)
        {
            using (new SecurityDisabler())
            {
                return linkField == null ? string.Empty : FieldUtility.GetGeneralLinkHref(linkField);
            }
        }

        public IStandardTemplateItem GetNextSibling(Item item)
        {
            if (!BucketManager.IsItemContainedWithinBucket(item))
            {
                var siblingItem = item.Axes.GetNextSibling();
                return siblingItem != null ? siblingItem.AsStronglyTyped() : null;
            }
            try
            {
                var index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(item));
                using (var context = index.CreateSearchContext())
                {
                    var bucketParent = item.GetParentBucketItemOrParent();
                    Assert.IsNotNull(bucketParent, "bucketParent != null");
                    var searchPredicate = PredicateBuilder.True<IStandardTemplateItem>();
                    searchPredicate = searchPredicate.And(p =>
                        p.AncestorIds.Contains(bucketParent.ID) &&
                        p.TemplateId != ID.Parse(Constants.BucketFolder) && p.Id != bucketParent.ID);
                    var nextSiblingItem =
                        context.GetSynthesisQueryable<IStandardTemplateItem>()
                            .Where(searchPredicate)
                            .OrderBy(result => result.Path)
                            .ToArray()
                            .SkipWhile(result => result.Id != item.ID)
                            .Skip(1)
                            .FirstOrDefault();

                    return nextSiblingItem;
                }
            }
            catch (Exception ex)
            {
                Log.Error("error getting next sibling", ex, this);
            }
            return null;
        }

        public IStandardTemplateItem GetNextSibling(IStandardTemplateItem item)
        {
            return item == null ? null : GetNextSibling(item.InnerItem);
        }

        public IStandardTemplateItem GetPreviousSibling(Item item)
        {
            if (!BucketManager.IsItemContainedWithinBucket(item))
            {
                var siblingItem = item.Axes.GetPreviousSibling();
                return siblingItem != null ? siblingItem.AsStronglyTyped() : null;
            }
            try
            {
                var index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(item));
                using (var context = index.CreateSearchContext())
                {
                    var bucketParent = item.GetParentBucketItemOrParent();
                    Assert.IsNotNull(bucketParent, "bucketParent != null");
                    var searchPredicate = PredicateBuilder.True<IStandardTemplateItem>();
                    searchPredicate = searchPredicate.And(p =>
                        p.AncestorIds.Contains(bucketParent.ID) &&
                        p.TemplateId != ID.Parse(Constants.BucketFolder) && p.Id != bucketParent.ID);
                    var previousSiblingItem =
                        context.GetSynthesisQueryable<IStandardTemplateItem>()
                            .Where(searchPredicate)
                            .OrderByDescending(result => result.Path)
                            .ToArray()
                            .SkipWhile(result => result.Id != item.ID)
                            .Skip(1)
                            .FirstOrDefault();

                    return previousSiblingItem;
                }
            }
            catch (Exception ex)
            {
                Log.Error("error getting next sibling", ex, this);
            }
            return null;
        }

        public IStandardTemplateItem GetPreviousSibling(IStandardTemplateItem item)
        {
            return item == null ? null : GetPreviousSibling(item.InnerItem);
        }
    }
}
