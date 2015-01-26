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
    public class TagController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tag
        public ActionResult Index()
        {
            return View(db.PostTags.ToList());
        }

        // GET: Tag/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostTag postTag = db.PostTags.Find(id);
            if (postTag == null)
            {
                return HttpNotFound();
            }
            return View(postTag);
        }

        // GET: Tag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,UrlSlug,Description")] PostTag postTag)
        {
            if (ModelState.IsValid)
            {
                db.PostTags.Add(postTag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(postTag);
        }

        // GET: Tag/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostTag postTag = db.PostTags.Find(id);
            if (postTag == null)
            {
                return HttpNotFound();
            }
            return View(postTag);
        }

        // POST: Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UrlSlug,Description")] PostTag postTag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postTag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(postTag);
        }

        // GET: Tag/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostTag postTag = db.PostTags.Find(id);
            if (postTag == null)
            {
                return HttpNotFound();
            }
            return View(postTag);
        }

        // POST: Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostTag postTag = db.PostTags.Find(id);
            db.PostTags.Remove(postTag);
            db.SaveChanges();
            return RedirectToAction("Index");
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
