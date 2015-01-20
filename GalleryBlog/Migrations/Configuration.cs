namespace GalleryBlog.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GalleryBlog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GalleryBlog.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            var aw = GetArtFromDirList(0);
            foreach (var item in aw)
            {
                var aName = item.Name;
                var artist = context.Artists.ToList().FirstOrDefault(a=>a.Name == aName);
                var aId = (artist == null) ? 1 : artist.Id;
                context.Artworks.AddOrUpdate(
                  p => p.Name,
                  new Models.Artwork
                  {
                      Name = item.ImageTitle,
                      ArtistId = aId,
                      Media = item.Media,
                      Size = item.Size,
                      ImageName = item.ImageName,
                      ImageAlt = item.ImageAlt,
                      ImageTitle = item.ImageTitle,
                      ImageDescription = item.ImageDescription,
                      CreatedDate = new DateTime(2000, 1, 1),
                      Price = item.Price,
                      Active = item.Active,
                      Sold = new DateTime(1800, 1, 1)
                  }

                );

















            }
        }

        private List<Models.Artwork> GetArtFromDirList(int idx = 0)
        {
            var gvm = new List<Models.Artwork>();
            var sDir = @"C:\dev\GalleryBlog\GalleryBlog\Content\Images\art";
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
                    var artistName = parts[0];
                    if (idx == 0 || (idx > 0 && id == idx))
                    {
                        gvm.Add(new Models.Artwork
                        {
                            ImageAlt = name,
                            ImageName = fn,
                            ImageDescription = artist + " " + desc,
                            Id = id,
                            ImageTitle = name,
                            Size = parts[3],
                            Name = artistName
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
