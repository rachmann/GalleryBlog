﻿using System;
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
    public class BlogController : Controller
    {
        private DataAccess db = new DataAccess();

        // GET: Blog
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Blog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.GetPost(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Subject,Body,Meta,UrlSlug,PostedOn,Approved,Modified,Published")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.AddPost(post);
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.GetPost(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Subject,Body,Meta,UrlSlug,PostedOn,Approved,Modified,Published")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.UpdatePost(post);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.GetPost(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.DeletePost(id);
            return RedirectToAction("Index");
        }

        // other helpers
        /// <summary>
        /// Return the posts belongs to a category.
        /// </summary>
        /// <param name="category">Url slug</param>
        /// <param name="p">Pagination number</param>
        /// <returns></returns>
        public ViewResult Category(string category, int p = 1)
        {
            var viewModel = new ListViewModel(db, category, "Category", p);

            if (viewModel.Category == null)
                throw new HttpException(404, "Category not found");

            ViewBag.Title = String.Format(@"Latest posts on category ""{0}""", viewModel.Category.Name);
            return View("List", viewModel);
        }

        /// <summary>
        /// Return the posts belongs to a tag.
        /// </summary>
        /// <param name="tag">Url slug</param>
        /// <param name="p">Pagination number</param>
        /// <returns></returns>
        public ViewResult Tag(string tag, int p = 1)
        {
            var viewModel = new ListViewModel(db, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new HttpException(404, "Tag not found");

            ViewBag.Title = String.Format(@"Latest posts tagged on ""{0}""", viewModel.Tag.Name);
            return View("List", viewModel);
        }

        /// <summary>
        /// Return a particular post based on the puslished year, month and url slug.
        /// </summary>
        /// <param name="year">Published year</param>
        /// <param name="month">Published month</param>
        /// <param name="title">Url slug</param>
        /// <returns></returns>
        public ViewResult Post(int year, int month, string title)
        {
            var post = db.GetPost(year, month, title);

            if (post == null)
                throw new HttpException(404, "Post not found");

            if (!post.Published.HasValue && !User.Identity.IsAuthenticated)
                throw new HttpException(401, "The post is not published");

            return View(post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}