using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalleryBlog.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public string Decription { get; set; }
        public string Image { get; set; }
        public decimal Rating { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime ModifiedDt { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}