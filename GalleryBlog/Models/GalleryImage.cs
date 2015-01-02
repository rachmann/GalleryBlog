using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalleryBlog.Models
{
    public class GalleryImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string ImageAlt { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        

    }
}