using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalleryBlog.Models
{
    public class Artwork
    {
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public String Name { get; set; }
        public String Media { get; set; }
        public String Size { get; set; }
        public String ImageName { get; set; }
        public string ImageAlt { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public double Price { get; set; }
        public bool Active { get; set; }
        public DateTime Sold { get; set; }
    }
}