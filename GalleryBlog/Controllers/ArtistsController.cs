using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GalleryBlog.Models;

namespace GalleryBlog.Controllers
{
    [Authorize (Roles="Administrator")]
    public class ArtistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artists
        public ActionResult Index()
        {
            return View(db.Artists.Select(a => new ArtistViewModel
            {
                Active = a.Active,
                ArtWorks = a.ArtWorks,
                Bio = a.Bio,
                ContainerDataCategory = a.ContainerDataCategory,
                ContainterClasses = a.ContainterClasses,
                Id = a.Id,
                Name = a.Name,
                Number = a.Number,
                ShortDescription = a.ShortDescription,
                ShowOnArtists = a.ShowOnArtists,
                ShowOnGallery = a.ShowOnGallery,
                Symbol = a.Symbol

            }).ToList());
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Active,ShowOnGallery,ShowOnArtists,Name,ShortDescription,Bio,Symbol,ContainterClasses,ContainerDataCategory,Number")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Active,ShowOnGallery,ShowOnArtists,Name,ShortDescription,Bio,Symbol,ContainterClasses,ContainerDataCategory,Number")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        protected List<Artwork> GetArtistArtwork(int id)
        {
            List<Artwork> model = new List<Artwork>();

            if (id > 0)
            {
                model.AddRange(db.Artworks.Where(a => a.ArtistId == id).ToList());
            }
            return model;
        }
        public JsonResult ArtWork(int id = 0)
        {
            var model = GetArtistArtwork(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddArtWork(int id = 0)
        {
            var model = GetArtistArtwork(id);

            var newArt = new Artwork
            {
                ArtistId = id,
                ImageName = "mjglogo.png",
                ImageAlt = string.Empty,
                ImageDescription = string.Empty,
                ImageTitle = string.Empty,
                Sold = new DateTime(1800, 1, 1),
                CreatedDate = new DateTime(1800, 1, 1),
                Media = string.Empty,
                Name = string.Empty,
                Price = 0,
                Size = string.Empty,
                Active = false
            };

            db.Artworks.Add(newArt);
            db.SaveChanges();
            model.Add(newArt);
            return PartialView("_ArtWorks", model);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<SelectListItem> ArtistsSelections()
        {
            var model = new List<SelectListItem>();

            model.AddRange(db.Artists.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }));

            return model;
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
