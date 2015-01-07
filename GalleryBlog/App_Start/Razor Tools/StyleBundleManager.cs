using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace GalleryBlog.Extensions
{
    
    public static class StyleBundleManager
    {
        private const string Key = "__StyleBundleManager__";

        /// <summary>
        /// Call this method from your partials and register your styles bundle.
        /// </summary>
        public static void RegisterStyles(this HtmlHelper htmlHelper, string styleBundleName)
        {
            //using a HashSet to avoid duplicate scripts.
            var set = htmlHelper.ViewContext.HttpContext.Items[Key] as HashSet<string>;
            if (set == null)
            {
                set = new HashSet<string>();
                htmlHelper.ViewContext.HttpContext.Items[Key] = set;
            }

            if (!set.Contains(styleBundleName))
                set.Add(styleBundleName);
        }

        /// <summary>
        /// In the bottom of your HTML document, most likely in the Layout file call this method.
        /// </summary>
        public static IHtmlString RenderStyles(this HtmlHelper htmlHelper)
        {
            var set = htmlHelper.ViewContext.HttpContext.Items[Key] as HashSet<string>;

            return set != null ? Styles.RenderFormat("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />", set.ToArray()) : MvcHtmlString.Empty;
        }


    }

}