using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Coveo.Framework.Log;
using Coveo.Framework.Processor;
using Coveo.SearchProvider.Pipelines;
using Coveo.SearchProvider.Processors;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace TomGrable.Website.Extensions.Pipelines.CoveoPostItemProcessing
{
    public class DataSourceHtmlSourceContentInBodyProcessor : IProcessor<CoveoPostItemProcessingPipelineArgs>
    {
        private static readonly ILogger _sLogger = CoveoLogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string SitecoreSystemFieldBeginning = "_";
        public const string BasicHtmlDateFormat = "dddd, MMMM d, yyyy";

        public bool IncludeFieldNames { get; set; }

        public bool IncludeTextFieldsOnly { get; set; }
        /// <summary>
        /// Sitecore field names: name of fields that should be added to the document's body
        /// also include field names for data source items
        /// </summary>
        public string FieldsToInclude { get; set; }

        /// <summary>
        /// Sitecore template names: list all templates that should be processed. 
        /// Include datasource item template names
        /// </summary>
        public string TemplatesToInclude { get; set; }

        private IEnumerable<string> FieldsToIncludeValues
        {
            get
            {
                return SplitValues(FieldsToInclude);
            }
        }

        private IEnumerable<string> TemplatesToIncludeValues
        {
            get
            {
                return SplitValues(TemplatesToInclude);
            }
        }

        public DataSourceHtmlSourceContentInBodyProcessor()
        {
            IncludeFieldNames = true;
            IncludeTextFieldsOnly = false;
        }
        public void Process(CoveoPostItemProcessingPipelineArgs args)
        {
            _sLogger.TraceEntering("Process");
            _sLogger.Debug("Entering the DataSourceHtmlSourceContentInBodyProcessor.");
            try
            {
                var item = (Item)(args.Item as SitecoreIndexableItem);
                // test it item is not null and exclude indexing for core database && !item.Database.Name.Equals("core", StringComparison.OrdinalIgnoreCase)
                if (item != null && args.CoveoItem.BinaryData == null)
                {
                    var htmlContentBuilder = new HtmlContentBuilder();
                    foreach (var pField in GetIncludedFields(args.Item.Fields))
                    {
                        if (pField.FieldType == typeof(DateField))
                        {
                            var s = pField.Value.ToString();
                            DateTime result;
                            if (DateTime.TryParse(s, CultureInfo.CurrentCulture,
                                DateTimeStyles.None, out result))
                                htmlContentBuilder.AddElement(pField.Name,
                                    result.ToString(BasicHtmlDateFormat), IncludeFieldNames);
                            else
                                _sLogger.Debug(
                                    "The date field with the value {0} will be ignored since it's an unknown date format.",
                                    s);
                        }
                        else
                            htmlContentBuilder.AddField(pField, IncludeFieldNames);
                    }
                    var dataSources =
                                      Globals.LinkDatabase.GetReferences(item)
                                          .Where(link => IsLayoutLink(link, item))
                                          .Select(link => link.GetTargetItem())
                                          .Where(targetItem => targetItem != null)
                                          .Distinct();
                    foreach (var dsItem in dataSources)
                    {
                        if (!IsTemplateIncluded(dsItem.TemplateName)) continue;
                        var indexableItem = (SitecoreIndexableItem)dsItem;
                        foreach (var pField in GetIncludedFields(indexableItem.Fields))
                        {
                            if (pField.FieldType == typeof(DateField))
                            {
                                var s = pField.Value.ToString();
                                DateTime result;
                                if (DateTime.TryParse(s, CultureInfo.CurrentCulture,
                                    DateTimeStyles.None, out result))
                                    htmlContentBuilder.AddElement(pField.Name,
                                        result.ToString(BasicHtmlDateFormat), IncludeFieldNames);
                                else
                                    _sLogger.Debug(
                                        "The date field with the value {0} will be ignored since it's an unknown date format.",
                                        s);
                            }
                            else
                                htmlContentBuilder.AddField(pField, IncludeFieldNames);
                        }
                    }
                    args.CoveoItem.BinaryData = Encoding.UTF8.GetBytes(htmlContentBuilder.GetHtml());

                }
            }
            catch (Exception ex)
            {
                _sLogger.Error(ex.Message, args);
            }
            _sLogger.TraceExiting("Process");
        }

        private bool IsTemplateIncluded(string pTemplateName)
        {
            _sLogger.TraceEntering("IsTemplateIncluded");
            bool flag =
                TemplatesToIncludeValues.Any(x => x.Equals(pTemplateName, StringComparison.OrdinalIgnoreCase));
            _sLogger.TraceExiting("IsTemplateIncluded");
            return flag;
        }
        private IEnumerable<IIndexableDataField> GetIncludedFields(IEnumerable<IIndexableDataField> pFields)
        {
            _sLogger.TraceEntering("GetIncludedFields");
            var source = pFields.Where(x => !x.Name.StartsWith(SitecoreSystemFieldBeginning));
            if (FieldsToIncludeValues.Any())
                source = source.Where(x => FieldsToIncludeValues.Contains(x.Name.ToLowerInvariant()));
            if (IncludeTextFieldsOnly)
                source = source.Where(x => x.FieldType == typeof(TextField));
            _sLogger.TraceExiting("GetIncludedFields");
            return source;
        }


        protected virtual bool IsLayoutLink(ItemLink link, Item sourceItem)
        {
            return link.SourceFieldID == FieldIDs.LayoutField && link.SourceDatabaseName == sourceItem.Database.Name;
        }


        private IEnumerable<string> SplitValues(string pValues)
        {
            _sLogger.TraceEntering("SplitValues");
            IEnumerable<string> enumerable = Enumerable.Empty<string>();
            if (!string.IsNullOrEmpty(pValues))
                enumerable =
                    pValues.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.ToLowerInvariant().Trim());
            _sLogger.TraceExiting("SplitValues");
            return enumerable;
        }
    }
}
