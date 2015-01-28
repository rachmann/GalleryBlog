using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GalleryBlog.Models;

namespace GalleryBlog.Controllers
{
    [Authorize(Roles="Administrator")]
    public class BlogController : Controller
    {
        private readonly DateTime EmptyDate = new DateTime(1800, 1, 1);
        private readonly DataAccess db = new DataAccess();

        // GET: Blog
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            // precreate the object so that autosave can work properly
            // the item is not published, so no need to worry as it's WIP
            var post = new Post
            {
                Body = string.Empty,
                Subject = string.Empty,
                Title = string.Empty,
                UrlSlug = string.Empty,
                Approved = EmptyDate,
                Meta = string.Empty,
                Modified = EmptyDate,
                PostedOn = EmptyDate,
                Published = EmptyDate
                
            };
            var recId = db.AddPost(post);
            return View(post);
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
                db.UpdatePost(post);
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
        // [ValidateInput(false)]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FroalaAutoSave(string body, int? id) // Do not change the parameter name from name 'body'
        {
            Post post = db.GetPost(id);
            post.Body = body;
            db.UpdatePost(post);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult FroalaUploadImage(HttpPostedFileBase file, int? postId) // Do not change the parameter name from name 'file'
        {

            var fileName = Path.GetFileName(file.FileName);
            var rootPath = Server.MapPath("~/Content/Images/blog/");
            file.SaveAs(Path.Combine(rootPath, fileName));

            return Json(new { link = "~/content/images/blog/" + fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FroalaMmImages() // Do not change the parameter name from name 'file'
        {
            var model = GetBlogArtFromDirList();
            return Json(model.ToArray(), JsonRequestBehavior.AllowGet);
        }


        private List<string> GetBlogArtFromDirList()
        {
            var gvm = new List<string>();
            var sDir = Server.MapPath("~/Content/Images/blog");
            
            
            var files = Directory.GetFiles(sDir);
            try
            {
                foreach (var f in files)
                {
                    var fn = f.Substring(f.LastIndexOf('\\') + 1);
                    gvm.Add(string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~/Content/Images/blog/" + fn)));
                }

            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return gvm;
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = db.GetPost(id);
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
        [AllowAnonymous]
        public ViewResult Tag(string tag, int p = 1)
        {
            var viewModel = new ListViewModel(db, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new HttpException(404, "Tag not found");

            ViewBag.Title = String.Format(@"Latest posts tagged on ""{0}""", viewModel.Tag.Name);
            return View("List", viewModel);
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
