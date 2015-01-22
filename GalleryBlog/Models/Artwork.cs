using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GalleryBlog.Models
{
    public class Artwork
    {
        public int Id { get; set; }
        [Display(Name = "Artist")]
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        public String Name { get; set; }
        public String Media { get; set; }
        public String Size { get; set; }
        [Display(Name = "Image Filename")]
        public String ImageName { get; set; }
        [Display(Name = "Not Visible")]
        public string ImageAlt { get; set; }
        [Display(Name = "Hover")]
        public string ImageTitle { get; set; }
        [Display(Name = "Description")]
        public string ImageDescription { get; set; }
        [Display(Name = "Created")]
        public DateTime CreatedDate { get; set; }
        public double Price { get; set; }
        public bool Active { get; set; }
        public DateTime Sold { get; set; }
    }
}