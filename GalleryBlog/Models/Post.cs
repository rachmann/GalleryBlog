﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryBlog.Models
{
        public class Post
        {
            public  int Id { get; set; }
            public  string Title { get; set; }
            [Display(Name = "Listing Subject or Description")]
            public  string Subject { get; set; }
            [AllowHtml]
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