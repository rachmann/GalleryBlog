#region Usings
using GalleryBlog.Models;
using System.Collections.Generic;
#endregion

namespace GalleryBlog.Models
{
    /// <summary>
    /// View model used for list view.
    /// </summary>
    /// <remarks>
    /// Same view model is used to list posts for category, tag or search text.
    /// </remarks>
    public class ListViewModel
    {
        public ListViewModel(DataAccess db, int p)
        {
            Posts = db.GetPosts(p-1, 10);
            TotalPosts = db.TotalPosts();
        }

        public ListViewModel(DataAccess db, string text, string type, int p)
        {

            switch (type)
            {
                case "Category":
                    Posts = db.GetPostsForCategory(text, p - 1, 10);
                    TotalPosts = db.TotalPostsForCategory(text);
                    Category = db.GetCategory(text);
                    break;

                case "Tag":
                    Posts = db.GetPostsForTag(text, p - 1, 10);
                    TotalPosts = db.TotalPostsForTag(text);
                    Tag = db.GetTag(text);
                    break;

                default:
                    Posts = db.GetPostsForSearch(text, p - 1, 10);
                    TotalPosts = db.TotalPostsForSearch(text);
                    Search = text;
                    break;
            }
        }

        public IList<Post> Posts
        { get; private set; }

        public int TotalPosts
        { get; private set; }

        public PostCategory Category
        { get; private set; }

        public PostTag Tag
        { get; private set; }

        public string Search
        { get; private set; }
    }
}