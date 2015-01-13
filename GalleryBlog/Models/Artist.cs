using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GalleryBlog.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public bool ShowOnGallery { get; set; }
        public bool ShowOnArtists { get; set; }
        
        public String Name { get; set; }
        [Display(Name="Quick Description")]
        public String ShortDescription { get; set; }
        public String Bio { get; set; }
        [Display(Name = "Artist Page Symbol")]
        public String Symbol { get; set; }
        [Display(Name = "All Categories")]
        public String ContainterClasses { get; set; }
        [Display(Name = "Main Category")]
        public String ContainerDataCategory { get; set; }
        [Display(Name = "Ref Num")]
        public String Number { get; set; }

        public virtual ICollection<Artwork> ArtWorks { get; set; }
    }
}