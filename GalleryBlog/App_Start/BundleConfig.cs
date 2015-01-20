using System.Web;
using System.Web.Optimization;

namespace GalleryBlog
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/gallery").Include(
                     "~/Content/gallery/imagesloaded.pkgd.min.js",
                     "~/Content/gallery/masonry.pkgd.min.js",
                     "~/Content/gallery/jquery.easing.1.3.js",
                     "~/Content/gallery/imagewall.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/artists").Include(
                      "~/Content/artists/isotope.pkg.min.js",
                      "~/Content/artists/artists.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/work").Include(
                      "~/Content/work/work.js"
                      ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/kendo/kendo.all.min.js",
                        "~/Scripts/kendo/endo.aspnetmvc.min.js",
                        "~/Scripts/kendo/kendo.web.min.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/kendo/kendo.common-bootstrap.min.css",
                      "~/Content/kendo/kendo.bootstrap.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
                      "~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/css_artists").Include(
                      "~/Content/artists/artists.css"
                      ));

        }
    }
}
