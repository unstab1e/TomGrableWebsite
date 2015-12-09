using Sitecore.Mvc.Presentation;
using Synthesis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Synthesis.Model;
using TomGrable.Website.Web.Models.Navigation;
using TomGrable.Website.Core.Interfaces;

namespace TomGrable.Website.Web.Controllers.Navigation
{
    public class NavigationController : Controller
    {
        private readonly INavigationService _navigationService;
        private readonly ISiteService _siteService;

        public NavigationController(INavigationService navigationService, ISiteService siteService) {
            _navigationService = navigationService;
            _siteService = siteService;
        }

        // GET: Navigation
        public ActionResult Index() {
            return View();
        }


        protected IPageBaseItem DataSourceItem = null;
        public ActionResult RenderHeaderMicroNavigation()
        {

            // Get the datasource item.
            var dataSourceItem = RenderingContext.Current.Rendering.Item.As<IStandardTemplateItem>();

            // Validate datasource item.
            if (dataSourceItem == null)
            {
                //if (Sitecore.Context.PageMode.IsPageEditorEditing)
                //{
                //    filterContext.Result = Content("<div class='alert'>No datasource has been set for this layer.</div>");
                //}
                //else
                //{
                //    Log.Warn("Data source item is null", this);
                //    filterContext.Result = new EmptyResult();
                //}

                //return;
                //return new EmptyResult();
            }

            // Ensure datasource is a layer.
            //DataSourceItem = dataSourceItem as IPageBaseItem;IFolderItem
            if (DataSourceItem == null)
            {
                //if (Sitecore.Context.PageMode.IsPageEditorEditing)
                //{
                //    filterContext.Result = Content("<div class='alert'>Datasource is not a Layer.</div>");
                //}
                //else
                //{
                //    Log.Warn("Data source item cannot be cast to IBaseLayerItem!", this);
                //    filterContext.Result = new EmptyResult();
                //}
            }

            //var dataFolder = RenderingContext.Current.Rendering.Item.As<IFolderItem>();
            ////var dataFolder = (IFolderItem)dataSourceItem;

            //var model = new List<INavigationBaseItem>();

            //if (null != dataFolder) {
            //    var nav = new HeaderMicroNavigationModel(dataFolder);
            //    model = nav.NavigationBaseItems;
            //}
            var model = new NavigationModel(_navigationService.GetNavigationItems());

            //return PartialView("~/Views/TomGrable/Components/Navigation/HeaderMicro.cshtml", model);
            return PartialView("~/Views/TomGrable/Components/Navigation/HeaderMicro.cshtml", model);
        }
    }


}