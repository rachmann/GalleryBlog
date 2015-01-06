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
            var model = GetArtFromDirList();

            return View(model);
        }

        public ActionResult Artists()
        {
            ViewBag.Message = "Your Artists page.";

            return View();
        }

        public ActionResult Work(int id = 0)
        {
            if (id == 0)
                return RedirectToActionPermanent("Index");
            
            ViewBag.Message = "Your item of work page.";

            var model = GetArtFromDirList(id);

            return View(model);
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

        private List<Models.GalleryImage> GetArtFromDirList(int idx = 0)
        {
            var gvm = new List<Models.GalleryImage>();
            var sDir = Server.MapPath(Url.Content("~/Content/Images/art/"));
            var files = Directory.GetFiles(sDir);
            if (idx > 0 && idx > files.Count())
            {
                return gvm;
            }

            try
            {
                var id = 1;
                foreach (var f in files)
                {

                    var fn = f.Substring(f.LastIndexOf('\\') + 1);
                    var parts = fn.Split('_');
                    var artist = "By " + parts[0] + ".";
                    var name = parts[1];
                    var desc = parts[2];
                    if (idx == 0 || (idx > 0 && id == idx))
                    {
                        gvm.Add(new GalleryImage
                        {
                            ImageAlt = name,
                            ImagePath = fn,
                            ImageDescription = artist + " " + desc,
                            Id = id,
                            ImageTitle = name
                        });
                    }
                    if (id == idx)
                    {
                        break;
                    }
                    id++;
                }

            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return gvm;
        }


    }
}