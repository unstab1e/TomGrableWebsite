using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Coveo.AbstractLayer.FieldManagement;
using Coveo.Framework.Log;
using Coveo.Framework.Processor;
using Coveo.SearchProvider.Pipelines;
using Coveo.SearchProvider.Processors;
using Sitecore.ContentSearch;
using Sitecore.Data.Fields;

namespace TomGrable.Website.Extensions.Pipelines.CoveoPostItemProcessing
{
    /// <summary>
    /// this is an exact copy of coveo's BasicHTML processor Added only for debugging purposes.
    /// </summary>
    public class BasicHtmlContentInBodyProcessor : IProcessor<CoveoPostItemProcessingPipelineArgs>
    {
        private static readonly ILogger s_Logger = CoveoLogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string SITECORE_SYSTEM_FIELD_BEGINNING = "_";
        public const string BASIC_HTML_DATE_FORMAT = "dddd, MMMM d, yyyy";

        public bool IncludeFieldNames { get; set; }

        public bool IncludeTextFieldsOnly { get; set; }

        public string FieldsToInclude { get; set; }

        public string TemplatesToInclude { get; set; }

        private IEnumerable<string> FieldsToIncludeValues
        {
            get
            {
                return this.SplitValues(this.FieldsToInclude);
            }
        }

        private IEnumerable<string> TemplatesToIncludeValues
        {
            get
            {
                return this.SplitValues(this.TemplatesToInclude);
            }
        }

        public BasicHtmlContentInBodyProcessor()
        {
            this.IncludeFieldNames = true;
            this.IncludeTextFieldsOnly = false;
        }

        public void Process(CoveoPostItemProcessingPipelineArgs p_Args)
        {
            s_Logger.TraceEntering("Process");
            s_Logger.Debug("Entering the BasicHtmlContentInBodyProcessor.");
            object obj;
            p_Args.CoveoItem.Metadata.TryGetValue(MetadataNames.s_TemplateName.OriginalFieldName, out obj);
            if (p_Args.CoveoItem.BinaryData == null && this.IsTemplateIncluded(obj as string))
            {
                HtmlContentBuilder htmlContentBuilder = new HtmlContentBuilder();
                p_Args.Item.LoadAllFields();
                foreach (IIndexableDataField p_Field in this.GetIncludedFields(p_Args.Item.Fields))
                {
                    if (p_Field.FieldType == typeof(DateField))
                    {
                        string s = p_Field.Value.ToString();
                        DateTime result;
                        if (DateTime.TryParse(s, (IFormatProvider)CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                            htmlContentBuilder.AddElement(p_Field.Name, (object)result.ToString("dddd, MMMM d, yyyy"), this.IncludeFieldNames);
                        else
                            s_Logger.Debug("The date field with the value {0} will be ignored since it's an unknown date format.", (object)s);
                    }
                    else
                        htmlContentBuilder.AddField(p_Field, this.IncludeFieldNames);
                }
                p_Args.CoveoItem.BinaryData = Encoding.UTF8.GetBytes(htmlContentBuilder.GetHtml());
            }
            s_Logger.TraceExiting("Process");
        }

        private bool IsTemplateIncluded(string p_TemplateName)
        {
            s_Logger.TraceEntering("IsTemplateIncluded");
            bool flag = true;
            if (Enumerable.Any<string>(this.TemplatesToIncludeValues))
                flag = Enumerable.Any<string>(this.TemplatesToIncludeValues, (Func<string, bool>)(x => x.Equals(p_TemplateName, StringComparison.OrdinalIgnoreCase)));
            s_Logger.TraceExiting("IsTemplateIncluded");
            return flag;
        }

        private IEnumerable<IIndexableDataField> GetIncludedFields(IEnumerable<IIndexableDataField> p_Fields)
        {
            s_Logger.TraceEntering("GetIncludedFields");
            IEnumerable<IIndexableDataField> source = Enumerable.Where<IIndexableDataField>(p_Fields, (Func<IIndexableDataField, bool>)(x => !x.Name.StartsWith("_")));
            if (Enumerable.Any<string>(this.FieldsToIncludeValues))
                source = Enumerable.Where<IIndexableDataField>(source, (Func<IIndexableDataField, bool>)(x => Enumerable.Contains<string>(this.FieldsToIncludeValues, x.Name.ToLowerInvariant())));
            if (this.IncludeTextFieldsOnly)
                source = Enumerable.Where<IIndexableDataField>(source, (Func<IIndexableDataField, bool>)(x => x.FieldType == typeof(TextField)));
            s_Logger.TraceExiting("GetIncludedFields");
            return source;
        }

        private IEnumerable<string> SplitValues(string p_Values)
        {
            s_Logger.TraceEntering("SplitValues");
            IEnumerable<string> enumerable = Enumerable.Empty<string>();
            if (!string.IsNullOrEmpty(p_Values))
                enumerable = Enumerable.Select<string, string>((IEnumerable<string>)p_Values.Split(new string[1]
        {
          ","
        }, StringSplitOptions.RemoveEmptyEntries), (Func<string, string>)(x => x.ToLowerInvariant().Trim()));
            s_Logger.TraceExiting("SplitValues");
            return enumerable;
        }
    }
}
