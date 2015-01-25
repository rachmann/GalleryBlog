using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryBlog.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }
    }
}