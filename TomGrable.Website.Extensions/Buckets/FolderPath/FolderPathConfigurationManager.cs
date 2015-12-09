using System.Xml;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Xml;

namespace TomGrable.Website.Extensions.Buckets.FolderPath
{
    public class FolderPathConfigurationManager
    {
        private static SafeDictionary<ID, string> _templateDateFieldCollection;

        private static SafeDictionary<ID, string> TemplateDateFieldCollection
        {
            get
            {
                return _templateDateFieldCollection;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                _templateDateFieldCollection = value;
            }
        }


        static FolderPathConfigurationManager()
        {
            Initialize();
        }
        private static void Initialize()
        {
            TemplateDateFieldCollection = new SafeDictionary<ID, string>();
            var masterDb = Context.ContentDatabase ?? Factory.GetDatabase("master");
            Assert.IsNotNull(masterDb, "content database is not defined");
            foreach (XmlNode node in Factory.GetConfigNodes("wacomBucketConfiguration/dateFieldMappings/mapping"))
            {

                var templateName = XmlUtil.GetAttribute("template", node);
                var fieldName = XmlUtil.GetAttribute("field", node);
                var template = masterDb.Templates[templateName];
                if (template != null && !string.IsNullOrEmpty(fieldName))
                {
                    TemplateDateFieldCollection.Add(template.ID, fieldName);
                }
                else
                {
                    Log.Info(string.Format("Could not find template : {0}", templateName), new object());
                }

            }
        }

        public static string GetDateFieldName(TemplateItem template)
        {
            return TemplateDateFieldCollection.ContainsKey(template.ID) ? TemplateDateFieldCollection[template.ID] : null;
        }
    }
}

