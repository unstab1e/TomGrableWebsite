using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetRenderingDatasource;
using Sitecore.Sites;
using Sitecore.Text;

namespace TomGrable.Website.Extensions.Pipelines.GetRenderingDatasource
{
    /// <summary>
    /// This GetRenderingDatasource pipeline processor creates the datasource location (if it doesn't already exist)
    /// using the template specified on the rendering whose datasource is being edited.
    /// </summary>
    public class DatasourceLocationTemplateProcessor
    {
        protected const string TEMPLATE_PARAMETER = "datasourceLocationTemplate";

        public virtual void Process(GetRenderingDatasourceArgs args)
        {
            Assert.IsNotNull(args, "args");
            RenderingItem rendering = new RenderingItem(args.RenderingItem);
            UrlString urlString = new UrlString(rendering.Parameters);
            var contentFolder = urlString.Parameters[TEMPLATE_PARAMETER];
            if (string.IsNullOrEmpty(contentFolder))
            {
                return;
            }
            if (!ID.IsID(contentFolder))
            {
                Log.Warn(string.Format("{0} for Rendering {1} contains improperly formatted ID: {2}", TEMPLATE_PARAMETER, args.RenderingItem.Name, contentFolder), this);
                return;
            }

            string text = args.RenderingItem["Datasource Location"];
            if (!string.IsNullOrEmpty(text))
            {
                if (text.StartsWith("./") && !string.IsNullOrEmpty(args.ContextItemPath))
                {
                    var itemPath = args.ContextItemPath + text.Remove(0, 1);
                    var item = args.ContentDatabase.GetItem(itemPath);
                    var contextItem = args.ContentDatabase.GetItem(args.ContextItemPath);
                    if (item == null && contextItem != null)
                    {
                        string itemName = text.Remove(0, 2);
                        //if we create an item in the current site context, the WebEditRibbonForm will see an ItemSaved event and think it needs to reload the page
                        using (new SiteContextSwitcher(SiteContextFactory.GetSiteContext("system")))
                        {
                            var datasourceLocationitem = contextItem.Add(itemName, new TemplateID(ID.Parse(contentFolder)));

                            // Set the sort order for the datasource location to 0 so it sorts to the top.
                            using (new EditContext(datasourceLocationitem))
                            {
                                datasourceLocationitem.Fields["__Sortorder"].Value = "0";
                            }
                        }
                    }
                }
            }
        }
    }
}
