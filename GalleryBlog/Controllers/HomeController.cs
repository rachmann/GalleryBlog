using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalleryBlog.Models;

namespace GalleryBlog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            ViewBag.Message = "The Gallery View.";
            ViewBag.Title = "Gallery View";
            var model = GetArtList();

            return View(model);
        }

        private List<Models.GalleryImage> GetArtList()
        {
            var gvm = new List<Models.GalleryImage>();

            var sDir = Server.MapPath(Url.Content("~/Content/Images/art/"));
            try
            {
                var id = 0;
                foreach (var f in Directory.GetFiles(sDir))
                {
                    var fn = f.Substring(f.LastIndexOf('\\')+1);
                    gvm.Add(new GalleryImage { ImageAlt = "Gallery Images", ImagePath = fn, ImageDescription = "Some description from DB", Id = ++id, ImageTitle = "Art" });
                }

            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return gvm;
        }

        public ActionResult Artists()
        {
            ViewBag.Message = "Your grouping of work page.";

            return View();
        }

        public ActionResult Work()
        {
            ViewBag.Message = "Your item of work page.";

            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.Message = "Your blog page.";

            return View();
        }

        public ActionResult Post()
        {
            ViewBag.Message = "Your blog post page.";

            return View();
        }

        public ActionResult About()
        {
            
            ViewBag.Title = "About";
            ViewBag.Message = "Mark's Gallery.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}