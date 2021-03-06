﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryBlog.Models
{
    public class ArtistListItem
    {
        public string ContainterClasses { get; set; }
        public string ContainerDataCategory { get; set; }
        [Display(Name="Name:")]
        public string ArtistName { get; set; }
        public string Symbol { get; set; }
        public string ImageName { get; set; }
        public String Number { get; set; }
        public Artwork Art { get; set; } // just the first piece of art you find
        
    }
}
