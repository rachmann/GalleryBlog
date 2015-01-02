using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalleryBlog.Models
{
    // this is not necessary
    // see CraftStore.Models.ItemConfiguration 
    //
    //public class PostTagMap
    //{
    //    // another way:[Index("IX_FirstAndSecond", 1, IsUnique = true)]
    //    [Key]
    //    [Column(Order = 0)]
    //    public int PostId { get; set; }
    //    // another way: [Index("IX_FirstAndSecond", 2, IsUnique = true)]
    //    [Key]
    //    [Column(Order = 1)]
    //    public int TagId { get; set; }
    //}
}