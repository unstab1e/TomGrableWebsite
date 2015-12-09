using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using Sitecore.Data;
using Sitecore.Mvc.Helpers;
using Sitecore.Mvc.Presentation;

namespace TomGrable.Website.Extensions.MvcExtensions.Html
{
    public static class HelperExtensions
    {
        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions)
        {
            return RawActionLink(ajaxHelper, linkText, actionName, controllerName, routeValues, ajaxOptions,
                new object());
        }
        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var repId = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repId, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repId, linkText));
        }
        public static MvcHtmlString RawActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues)
        {
            return RawActionLink(htmlHelper, linkText, actionName, controllerName, routeValues,
                new object());
        }
        public static MvcHtmlString RawActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            var repId = Guid.NewGuid().ToString();
            var lnk = htmlHelper.ActionLink(repId, actionName, controllerName, routeValues, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repId, linkText));
        }
    }
}
