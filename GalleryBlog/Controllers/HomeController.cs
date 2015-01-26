using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using GalleryBlog.Models;
using System.Configuration;

namespace GalleryBlog.Controllers
{
    public class HomeController : Controller
    {
        private DataAccess db = new DataAccess();

        public ActionResult Index()
        {
            if (TempData["ConfirmReminder"] != null)
            {
                TempData.Remove("ConfirmReminder");
                ViewBag.ConfirmReminder = 1;
            }
            ViewBag.Posts = db.GetPosts(0, 3);
            return View();
        }

        public ActionResult Gallery()
        {
            ViewBag.Message = "The Gallery View.";
            ViewBag.Title = "Gallery View";
            var model = GetArtFromDb();

            return View(model);
        }

        public ActionResult Artists()
        {
            ViewBag.Message = "Your Artists page.";

            var model = GetArtistList();
            return View(model);
        }

        public ActionResult Artist(int id = 0)
        {
            if (id == 0)
                return RedirectToActionPermanent("Index");

            ViewBag.Message = "Artist page";
            var num = id.ToString(CultureInfo.InvariantCulture);
            var model = db.Artists.FirstOrDefault(l => l.Number == num);

            return View(model);
        }

        public ActionResult Work(int id = 0)
        {
            if (id == 0)
                return RedirectToActionPermanent("Index");

            ViewBag.Message = "Your item of work page.";

            var model = GetArtFromDb(id);

            return View(model);
        }

        public ActionResult About()
        {

            ViewBag.Title = "About";
            ViewBag.Message = "Mark's Gallery.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "The gallery contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(string messageName, string messageEmail, string messageBody)
        {
            var sender = new SendMailerController();

            if(sender.SendAdminContactPageEmail(messageName, messageEmail, messageBody, ConfigurationManager.AppSettings["AdminEmailCC"], Request.Url.Scheme, Request.Url.Authority))
            {
                return View("Thanks");
            }

            return RedirectToActionPermanent("Index", "Home");
        }

        private List<Models.Artwork> GetArtFromDb(int idx = 0)
        {
            var oneway = false;
            List<Models.Artwork> artworks;
            if (oneway)
            {
                artworks =
                    db.Artists.Where(a => a.Active && a.ShowOnGallery)
                        .ToList()
                        .SelectMany(artist => artist.ArtWorks.Where(artwork => artwork.Active),
                            (artist, artwork) => new Artwork
                            {
                                Name = artist.Name,
                                ArtistId = artwork.ArtistId,
                                Active = artwork.Active,
                                CreatedDate = artwork.CreatedDate,
                                Id = artwork.Id,
                                ImageAlt = artwork.ImageAlt,
                                ImageDescription = artwork.ImageDescription,
                                ImageName = artwork.ImageName,
                                ImageTitle = artwork.ImageTitle,
                                Media = artwork.Media,
                                Price = artwork.Price,
                                Size = artwork.Size,
                                Sold = artwork.Sold
                            }).ToList();
            }
            else
            {

                artworks =
                    db.Artists.Where(a => a.Active && a.ShowOnGallery)
                        .SelectMany(artist => artist.ArtWorks.Where(aw=> (idx == 0 || aw.Id == idx))).ToList()
                        .Select(w => new Artwork()
                        {
                            Name = w.Artist.Name,
                            ArtistId = w.ArtistId,
                            Active = w.Active,
                            CreatedDate = w.CreatedDate,
                            Id = w.Id,
                            ImageAlt = w.ImageAlt,
                            ImageDescription = w.ImageDescription,
                            ImageName = w.ImageName,
                            ImageTitle = w.ImageTitle,
                            Media = w.Media,
                            Price = w.Price,
                            Size = w.Size,
                            Sold = w.Sold
                        }).ToList();
            }
            return artworks;
        }

        private List<Models.Artwork> GetArtFromDirList(int idx = 0)
        {

            var gvm = new List<Artwork>();
            var sDir = Server.MapPath(Url.Content("~/Content/Images/art/"));
            var files = Directory.GetFiles(sDir);
            if (idx > 0 && idx > files.Count())
            {
                return gvm;
            }

            try
            {
                var id = 59;
                foreach (var f in files)
                {

                    var fn = f.Substring(f.LastIndexOf('\\') + 1);
                    var parts = fn.Split('_');

                    var artistRec = db.Artists.ToList().FirstOrDefault(a => a.Name == parts[0]);
                    if (artistRec != null)
                        id = artistRec.Id;

                    var artist = "By " + parts[0] + ".";
                    var name = parts[1];
                    var desc = parts[2];
                    if (idx == 0 || (idx > 0 && id == idx))
                    {
                        gvm.Add(new Artwork
                        {
                            Name = name,
                            ImageAlt = name,
                            ImageName = fn,
                            ImageDescription = artist + " " + desc,
                            Id = id,
                            Size = parts[3],
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


        private List<ArtistListItem> GetArtistList()
        {
            //int i=0;
            var items = db.Artists.Where(a => a.Active && a.ShowOnArtists)
                .Select(a => new ArtistListItem
                {
                    ArtistName = a.Name,
                    ContainerDataCategory = a.ContainerDataCategory,
                    ContainterClasses = a.ContainterClasses,
                    Symbol = a.Symbol,
                    Number = a.Number,
                    Art = a.ArtWorks.FirstOrDefault()
                }).ToList();

            return items;
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

        /// <summary>
        /// Return the list page with latest posts.
        /// </summary>
        /// <param name="p">pagination number</param>
        /// <returns></returns>
        public ViewResult Posts(int p = 1)
        {
            var viewModel = new ListViewModel(db, p);
            ViewBag.Title = "Latest Posts";
            return View("Blog", viewModel);
        }

        /// <summary>
        /// Return the posts belongs to a category.
        /// </summary>
        /// <param name="category">Url slug</param>
        /// <param name="p">Pagination number</param>
        /// <returns></returns>
        /// 
        public ViewResult Category(string category, int p = 1)
        {
            var viewModel = new ListViewModel(db, category, "Category", p);

            if (viewModel.Category == null)
                throw new HttpException(404, "Category not found");

            ViewBag.Title = String.Format(@"Latest posts on category ""{0}""", viewModel.Category.Name);
            return View("Blog", viewModel);
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
            return View("Blog", viewModel);
        }

        /// <summary>
        /// Return the posts that matches the search text.
        /// </summary>
        /// <param name="s">search text</param>
        /// <param name="p">Pagination number</param>
        /// <returns></returns>
        public ViewResult Search(string s, int p = 1)
        {
            ViewBag.Title = String.Format(@"Lists of posts found for search text ""{0}""", s);

            var viewModel = new ListViewModel(db, s, "Search", p);
            return View("Blog", viewModel);
        }

        /// <summary>
        /// Child action that returns the sidebar partial view.
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult Sidebars()
        {
            var widgetViewModel = new WidgetViewModel(db);
            return PartialView("_Sidebars", widgetViewModel);
        }

        /// <summary>
        /// Generate and return RSS feed.
        /// </summary>
        /// <returns></returns>
        public ActionResult Feed()
        {
            var blogTitle = ConfigurationManager.AppSettings["BlogTitle"];
            var blogDescription = ConfigurationManager.AppSettings["BlogDescription"];
            var blogUrl = ConfigurationManager.AppSettings["BlogUrl"];

            var posts = db.GetPosts(0, 25).Select
            (
                p => new SyndicationItem
                    (
                        p.Title,
                        p.Subject,
                        new Uri(string.Concat(blogUrl, p.Href(Url)))
                    )
            );

            var feed = new SyndicationFeed(blogTitle, blogDescription, new Uri(blogUrl), posts)
            {
                Copyright = new TextSyndicationContent(String.Format("Copyright © {0}", blogTitle)),
                Language = "en-US"
            };

            return new FeedResult(new Rss20FeedFormatter(feed));
        }

    }
}