using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GalleryBlog.Models;
using System.IO;
using System.Web.Helpers;

namespace GalleryBlog.Controllers
{
    public class ArtworksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artworks
        public ActionResult Index()
        {
            return View(db.Artworks.ToList());
        }

        // GET: Artworks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artwork artwork = db.Artworks.Find(id);
            if (artwork == null)
            {
                return HttpNotFound();
            }
            return View(artwork);
        }

        // GET: Artworks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artworks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ArtistId,Name,Media,Size,ImageName,ImageAlt,ImageTitle,ImageDescription,CreatedDate,Price,Active,Sold")] Artwork artwork)
        {
            if (ModelState.IsValid)
            {
                db.Artworks.Add(artwork);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artwork);
        }

        // GET: Artworks/Edit/5
        public ActionResult Edit(int? id)
        {
            ArtworkViewModel awvm;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aw = db.Artworks.Find(id);

            if (aw == null)
            {
                return HttpNotFound();
            }
            else
            {
                awvm = new ArtworkViewModel
                {
                    Id = aw.Id,
                    ArtistId = aw.ArtistId,
                    Name = aw.Name,
                    Media = aw.Media,
                    Size = aw.Size,
                    ImageName = aw.ImageName,
                    ImageAlt = aw.ImageAlt,
                    ImageTitle = aw.ImageTitle,
                    ImageDescription = aw.ImageDescription,
                    CreatedDate = aw.CreatedDate,
                    Price = aw.Price,
                    Active = aw.Active,
                    Sold = aw.Sold
                };
                awvm.Artists = db.Artists.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();
            }
            return View(awvm);
        }

        // POST: Artworks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ArtistId,Name,Media,Size,ImageName,ImageAlt,ImageTitle,ImageDescription,CreatedDate,Price,Active,Sold")] Artwork artwork)
        {
            if (ModelState.IsValid)
            {
                var WatermarkPath = Path.Combine(Server.MapPath("~/Content/Images/art"), "mjglogo.png");
                var physicalPath = Path.Combine(Server.MapPath("~/Content/Images/art"), artwork.ImageName);
                var elementPath = Path.Combine(Server.MapPath("~/Content/Images/elements"), artwork.ImageName);
                var thumbPath = Path.Combine(Server.MapPath("~/Content/Images/thumb"), artwork.ImageName);

                var wimg = new WebImage(physicalPath);

                // first add watermark
                wimg.AddImageWatermark(WatermarkPath, width: 300, height: 300, opacity: 11, padding: 100);

                // Write image with watermark
                wimg.Save(physicalPath);

                // element design
                var iHeight = wimg.Height;
                var iWidth = wimg.Width;
                double endW, endH;
                var startH = iHeight * .2;
                if ((startH + 300) > iHeight)
                {
                    endH = iHeight;
                }
                else
                {
                    endH = iHeight - (startH + 300);
                }

                var startW = iWidth * .2;
                if ((startW + 300) > iWidth)
                {
                    endW = iWidth;
                }
                else
                {
                    endW = iWidth - (startW + 300);
                }
                // Wrtie element image
                wimg.Crop(int.Parse(startH.ToString("F0")), int.Parse(startW.ToString("F0")), int.Parse(endH.ToString("F0")), int.Parse(endW.ToString("F0")));
                wimg.Save(elementPath);

                // Design thumb
                wimg = new WebImage(physicalPath);
                // first add watermark
                wimg.AddImageWatermark(WatermarkPath, width: 300, height: 300, opacity: 11, padding: 100);
                wimg.Resize(200, 200, true, true);
                // Write thumb out
                wimg.Save(thumbPath);
                             

                db.Entry(artwork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Artists", new { id = artwork.ArtistId });
            }
            return View(artwork);
        }

        // GET: Artworks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artwork artwork = db.Artworks.Find(id);
            if (artwork == null)
            {
                return HttpNotFound();
            }
            return View(artwork);
        }

        // POST: Artworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artwork artwork = db.Artworks.Find(id);
            db.Artworks.Remove(artwork);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PhotoUpload(HttpPostedFileBase photo, int Id, string f = null)
        {
            if (photo != null)
            {
                string picFN = System.IO.Path.GetFileName(photo.FileName);


                string path = System.IO.Path.Combine(Server.MapPath("~/Content/Images/art"), picFN);
                // file is uploaded
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                photo.SaveAs(path);

                TempData["ImagePath"] = picFN;
                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    photo.InputStream.CopyTo(ms);
                //    byte[] array = ms.GetBuffer();
                //}

            }
            // after successfully uploading redirect the user
            string action = "Edit";
            if (!string.IsNullOrWhiteSpace(f))
            {
                action = f;
            }
            return RedirectToAction(f, "Artworks", new { id = Id });
        }


        public ActionResult SaveImage(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Images/art"), fileName);

                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult RemoveImage(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Images/art"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
