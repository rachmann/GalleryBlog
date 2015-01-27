using System.Web;
using System.Web.Optimization;

namespace GalleryBlog
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.11.2.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"
                        ));

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

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                        "~/Scripts/kendo/kendo.all.min.js",
                        "~/Scripts/kendo/endo.aspnetmvc.min.js",
                        "~/Scripts/kendo/kendo.web.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/respond").Include(
                        "~/Scripts/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.8.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/froala").Include(
                "~/Scripts/froala/froala_editor.min.js",
                "~/Scripts/froala/plugins/lists.min.js",
                "~/Scripts/froala/plugins/colors.min.js",
                "~/Scripts/froala/plugins/font_family.min.js",
                "~/Scripts/froala/plugins/font_size.min.js",
                "~/Scripts/froala/plugins/media_manager.min.js",
                "~/Scripts/froala/plugins/file_upload.min.js",
                "~/Scripts/froala/plugins/tables.min.js",
                "~/Scripts/froala/plugins/block_styles.min.js",
                "~/Scripts/froala/langs/en_ca.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/slider").Include(
                      "~/Scripts/slider/jquery.easing.1.2.js",
                      "~/Scripts/slider/jquery.anythingslider.min.js",
                      "~/Scripts/slider/jquery.anythingslider.fx.min.js",
                      "~/Scripts/slider/swfobject.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/css_kendo").Include(
                      "~/Content/kendo/kendo.common-bootstrap.min.css",
                      "~/Content/kendo/kendo.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css_bootstrap").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css_fontawesome").Include(
                      "~/Content/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/Content/css_artists").Include(
                      "~/Content/artists/artists.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/css_froala").Include(
                      "~/Content/froala/froala_editor.min.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/css_slider").Include(
                       "~/Content/slider/anythingslider.css",
                       "~/Content/slider/slider-custom.css"
                      ));

        }
    }
}
