using GalleryBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryBlog
{
    public static class SyndicationExtension
    {
        public static string Href(this Post post, UrlHelper helper)
        {
            return helper.RouteUrl(new
            {
                controller = "Home",
                action = "Post",
                year = post.PostedOn.Year,
                month = post.PostedOn.Month,
                title = post.UrlSlug
            });
        }
    }
}