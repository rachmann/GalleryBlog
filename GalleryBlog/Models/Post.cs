using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalleryBlog.Models
{
        public class Post
        {
            public  int Id { get; set; }
            public  string Title { get; set; }
            public  string Subject { get; set; }
            public  string Body { get; set; }
            public  string Meta { get; set; }
            public  string UrlSlug { get; set; }
            public virtual DateTime PostedOn { get; set; }
            public DateTime? Approved { get; set; }
            public virtual DateTime? Modified { get; set; }
            public DateTime? Published { get; set; }
            
            public virtual ICollection<PostCategory> Categories { get; set; }
            public virtual ICollection<PostTag> Tags { get; set; }
        }
    
}