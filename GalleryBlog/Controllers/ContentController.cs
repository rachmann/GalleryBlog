using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryBlog.Controllers
{
    public class ContentController : Controller
    {
        // GET: Content
        [Authorize(Roles="Administrator")]
        public ActionResult Index()
        {
            return View();
        }
    }
}