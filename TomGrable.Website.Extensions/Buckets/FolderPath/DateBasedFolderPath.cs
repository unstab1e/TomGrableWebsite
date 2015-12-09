using System;
using Sitecore;
using Sitecore.Buckets.Util;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace TomGrable.Website.Extensions.Buckets.FolderPath
{
    public class DateBasedFolderPath : IDynamicBucketFolderPath
    {

        protected DateTime EnsureAndGetDate(Item item, string dateFieldName, DateTime defaultDateTime)
        {
            DateField dateField = item.Fields[dateFieldName];
            if (dateField == null)
                return defaultDateTime;
            var result = dateField.DateTime;
            if (!string.IsNullOrEmpty(dateField.InnerField.Value))
            {
                return result;
            }
            using (new EditContext(item))
            {
                dateField.Value = DateUtil.ToIsoDate(defaultDateTime);
                return DateUtil.IsoDateToDateTime(dateField.InnerField.Value);
            }
        }

        public string GetFolderPath(Database database, string name, ID templateId, ID newItemId, ID parentItemId, DateTime creationDateOfNewItem)
        {
            var newItem = database.GetItem(newItemId);
            if (newItem == null)
                return creationDateOfNewItem.ToString(BucketConfigurationSettings.BucketFolderPath, Context.Culture);
            var fieldName = FolderPathConfigurationManager.GetDateFieldName(newItem.Template);
            if (string.IsNullOrEmpty(fieldName))
                return creationDateOfNewItem.ToString(BucketConfigurationSettings.BucketFolderPath, Context.Culture);
            var creationDateTime = EnsureAndGetDate(newItem, fieldName, creationDateOfNewItem);
            return creationDateTime.ToString(BucketConfigurationSettings.BucketFolderPath, Context.Culture);
        }
    }
}