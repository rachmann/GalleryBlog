using GalleryBlog.Models;
using System.Collections.Generic;

namespace GalleryBlog.Models
{
  /// <summary>
  /// View model used to wrap data for sidebar widgets.
  /// </summary>
  public class WidgetViewModel
  {
    public WidgetViewModel(DataAccess blogRepository)
    {
      Categories = blogRepository.GetCategories();
      Tags = blogRepository.GetTags();
      LatestPosts = blogRepository.GetPosts(0, 10);
    }

    public IList<PostCategory> Categories 
    { get; private set; }

    public IList<PostTag> Tags 
    { get; private set; }

    public IList<Post> LatestPosts 
    { get; private set; }
  }
}