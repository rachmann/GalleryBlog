﻿using System.Web;
using System.Web.Optimization;

namespace GalleryBlog
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery-{version}.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                      "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/jquery.validate.unobtrusive.min.js"
                      ));

            // "Single Responsibility" design - keep parts for any tools separate to include when you need them
            bundles.Add(new ScriptBundle("~/bundles/imagewall").Include(
                      "~/Content/imagewall/imagesloaded.pkgd.min.js",
                      "~/Content/imagewall/masonry.pkgd.min.js",
                      "~/Content/imagewall/jquery.easing.1.3.js",
                      "~/Content/imagewall/imagewall.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/artists").Include(
                      "~/Content/isotope/isotope.pkg.min.js",
                      "~/Content/isotope/artists.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/work").Include(
                      "~/Content/work/work.js"
                      ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                      "~/Scripts/modernizr-{version}.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/animate.css",
                      "~/Content/Site.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/artists").Include(
                      "~/Content/isotope/artists.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/work").Include(
                      "~/Content/work/rating.css",
                      "~/Content/work/work.css"
                      ));

            // "Single Responsibility" design - keep parts for any tools separate to include when you need them
            bundles.Add(new StyleBundle("~/Content/imagewall").Include(
                      "~/Content/imagewall/imagewall.css"
                      ));

        }
    }
}
