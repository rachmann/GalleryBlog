using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryBlog.Extensions
{
    public static class NavHelper
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            var cssClass = "active";
            var currentAction = (string)html.ViewContext.RouteData.Values["action"];
            var currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }
    }
}