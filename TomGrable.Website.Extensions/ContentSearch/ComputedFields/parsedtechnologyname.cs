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
    public class ParsedTechnologyName : IComputedIndexField
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
                //    return GetTechnoloyNames(stItem.InnerItem.As<ITaxonomyItem>().Technologies.TargetItems);
                //}
            }
            catch (Exception ex)
            {
                Log.Error("ParsedTechnologyName", ex, this);
            }
            return null;
        }
        //private List<string> GetTechnoloyNames(IEnumerable<IStandardTemplateItem> technologyItems)
        //{
        //    if (technologyItems.Any())
        //        return technologyItems.Where(item => item is ILookupValueItem)
        //            .Select(item => StringUtil.GetString(item.InnerItem.As<ILookupValueItem>().Title, item.DisplayName).Trim()).ToList();
        //    return null;
        //}
        public string FieldName { get; set; }

        public string ReturnType
        {
            get;
            set;
        }
    }
}
