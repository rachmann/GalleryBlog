using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GalleryBlog.Models
{
    public class ArtistViewModel
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        [Display(Name = "On Gallery")]
        public bool ShowOnGallery { get; set; }
        [Display(Name = "On Artists")]
        public bool ShowOnArtists { get; set; }

        public String Name { get; set; }
        [Display(Name = "Quick Description")]
        public String ShortDescription { get; set; }
        public String Bio { get; set; }
        public String ShortBio
        {
            get
            {
                return (Bio == null ? string.Empty : Bio.Substring(0, 20)) + "...";
            }
        }
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