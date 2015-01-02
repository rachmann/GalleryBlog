using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalleryBlog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<Item> Items { get; set; }
 
    }
}