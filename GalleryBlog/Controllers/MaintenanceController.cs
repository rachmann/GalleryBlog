using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryBlog.Controllers
{
    public class MaintenanceController : Controller
    {
        // GET: Maintenance
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }
    }
}