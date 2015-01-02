using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalleryBlog.Models
{
    public class PostTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}