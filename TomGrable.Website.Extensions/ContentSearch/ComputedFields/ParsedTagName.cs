using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Diagnostics;
using Synthesis;
using Synthesis.Model;

namespace TomGrable.Website.Extensions.ContentSearch.ComputedFields
{
    public class ParsedTagName : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            try
            {
                //Assert.ArgumentNotNull(indexable, "indexable");
                //var sitecoreIndexableItem = (SitecoreIndexableItem)indexable;
                //Assert.ArgumentNotNull(sitecoreIndexableItem, "sitecoreIndexableItem");
                //var stItem = sitecoreIndexableItem.Item.As<IStandardTemplateItem>();
                //if (stItem is ITaxonomyItem)
                //{
                //    return GetTagNames(stItem.InnerItem.As<ITaxonomyItem>().Tags.TargetItems);
                //}
            }
            catch (Exception ex)
            {
                Log.Error("ParsedTagName", ex, this);
            }
            return null;

        }

        //private List<string> GetTagNames(IEnumerable<IStandardTemplateItem> tagsItems)
        //{
        //    if (tagsItems.Any())
        //        return tagsItems.Where(item => item is ITagValueItem)
        //            .Select(item => StringUtil.GetString(item.InnerItem.As<ITagValueItem>().Title, item.DisplayName).Trim()).ToList();
        //    return null;
        //}
        public string FieldName { get; set; }

        public string ReturnType { get; set; }
    }
}