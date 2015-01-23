﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalleryBlog.Models;
using System.Configuration;

namespace GalleryBlog.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (TempData["ConfirmReminder"] != null)
            {
                TempData.Remove("ConfirmReminder");
                ViewBag.ConfirmReminder = 1;
            }
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

        [HttpPost]
        public ActionResult Contact(string messageName, string messageEmail, string messageBody)
        {
            var sender = new SendMailerController();

            sender.SendAdminContactPageEmail(messageName, messageEmail, messageBody, ConfigurationManager.AppSettings["AdminEmailCC"], Request.Url.Scheme, Request.Url.Authority);

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
            //var list = new List<ArtistListItem>
            //{
            //    new ArtistListItem
            //    {
            //        ArtistName = "Mark Gleberson",
            //        Symbol = "Mg",
            //        ContainterClasses = "photo mixedmedia",
            //        ContainerDataCategory = "photo",
            //        ImageName = "Mark Gleberson_The Delivery_Oil__.jpeg",
            //        Number = "6",
            //        ImageSize = "30x40"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Robert Shuttleworth",
            //        Symbol = "Rs",
            //        ContainterClasses = "sculpture",
            //        ContainerDataCategory = "sculpture",
            //        ImageName = "Robert Shuttleworth_Pee Wee_Mixed media sculpture_48x90_.jpg",
            //        Number = "57",
            //        ImageSize = "48x90"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Eryn O'Neill",
            //        Symbol = "EoN",
            //        ContainterClasses = "photo",
            //        ContainerDataCategory = "photo",
            //        ImageName = "Eryn ONeill_Window Shopping_Oil on canvas_16x32_.JPG",
            //        Number = "92",
            //        ImageSize = "16x32"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Chris Albert",
            //        Symbol = "Ca",
            //        ContainterClasses = "photo",
            //        ContainerDataCategory = "photo",
            //        ImageName = "Chris Albert_St Lawrence Market_Photograph on panel with resin overlay_30x48_.jpg",
            //        Number = "208",
            //        ImageSize = "30x48"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Laura Culic",
            //        Symbol = "Lc",
            //        ContainterClasses = "photo",
            //        ContainerDataCategory = "photo",
            //        ImageName = "Ivano Stocco_Crossroads_Mixed media painting on panel_48x60_.JPG",
            //        Number = "42",
            //        ImageSize = "48x60"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Carol Westcott",
            //        Symbol = "Cw",
            //        ContainterClasses = "painting",
            //        ContainerDataCategory = "painting",
            //        ImageName = "Eryn ONeill_On The Highline_Oil on canvas_16x32_.JPG",
            //        Number = "113",
            //        ImageSize = "16x32"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Ian Busher",
            //        Symbol = "Ib",
            //        ContainterClasses = "mixedmedia",
            //        ContainerDataCategory = "mixedmedia",
            //        ImageName = "Ian Busher_Ive Seen It Happen_mixed media on gate_30x73_.JPG",
            //        Number = "12",
            //        ImageSize = "30x73"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Andrea Rinaldo",
            //        Symbol = "Ar",
            //        ContainterClasses = "painting",
            //        ContainerDataCategory = "painting",
            //        ImageName = "Andrea Rinaldo_White Neon Blush_mixed media on canvas_48x60_.jpg",
            //        Number = "79",
            //        ImageSize = "48x60"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Marjolyn van der Hart",
            //        Symbol = "MvdH",
            //        ContainterClasses = "painting",
            //        ContainerDataCategory = "painting",
            //        ImageName = "Marjolyn van der Hart_She Wants To Know_Mixed media painting on panel_40x40_.JPG",
            //        Number = "19",
            //        ImageSize = "40x40"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Beverly Jenkins",
            //        Symbol = "Bj",
            //        ContainterClasses = "painting",
            //        ContainerDataCategory = "painting",
            //        ImageName = "Ivano Stocco_Gridiron_Mixed media painting on panel_36x30_.JPG",
            //        Number = "4",
            //        ImageSize = ""
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Ivano Stocco",
            //        Symbol = "Is",
            //        ContainterClasses = "sculpture mixedmedia",
            //        ContainerDataCategory = "sculpture",
            //        ImageName = "Ivano Stocco_Nightlife_Mixed media painting on panel_36x48_.JPG",
            //        Number = "81",
            //        ImageSize = "36x48"
            //    },
            //    new ArtistListItem
            //    {
            //        ArtistName = "Tommy Vohs",
            //        Symbol = "Tv",
            //        ContainterClasses = "mixedmedia",
            //        ContainerDataCategory = "mixedmedia",
            //        ImageName = "Tommy Vohs_Rocket Launch_iPhone-graphic image on aluminum dibond_40x40_.JPG",
            //        Number = "48",
            //        ImageSize = "40x40"
            //    }
            //};
            //
            // return list;
        }

    }
}