using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryBlog.Models
{
    public class DataAccess
    {
        private readonly DateTime oldDate = new DateTime(1867, 7, 1);

        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public DbSet<Artist> Artists
        {
            get { return _db.Artists; }
        }
        public DbSet<Artwork> Artworks
        {
            get { return _db.Artworks; }
        }
        public DbSet<CartItem> CartItems
        {
            get { return _db.CartItems; }
        }
        public DbSet<Category> Categories
        {
            get { return _db.Categories; }
        }
        public DbSet<Item> Items
        {
            get { return _db.Items; }
        }
        public DbSet<Post> Posts
        {
            get { return _db.Posts; }
        }
        public DbSet<PostCategory> PostCategories
        {
            get { return _db.PostCategories; }
        }
        public DbSet<PostTag> PostTags
        {
            get { return _db.PostTags; }
        }

        #region Post and Tag Logic

        /// <summary>
        /// Return collection of posts based on pagination parameters.
        /// </summary>
        /// <param name="pageNo">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public List<Post> GetPosts(int pageNo, int pageSize)
        {
            
            var posts = Posts.Where(p => p.Published.HasValue && p.Published.Value > oldDate)
                                  .OrderByDescending(p => p.PostedOn)
                                  .Skip(pageNo * pageSize)
                                  .Take(pageSize).ToList();
            return posts;
        }

        /// <summary>
        /// Return collection of posts belongs to a particular tag.
        /// </summary>
        /// <param name="tagSlug">Tag's url slug</param>
        /// <param name="pageNo">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public List<Post> GetPostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            var posts = _db.Posts
                            .Where(p => p.Published.HasValue && p.Published.Value > oldDate && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                            .OrderByDescending(p => p.PostedOn)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

            //var x = _db.Database.Connection.Query<Post>()
            //.Where(p => postIds.Contains(p.Id))
            //.OrderByDescending(p => p.PostedOn)
            //.FetchMany(p => p.Tags)
            //.ToList();

            return posts;
        }

        /// <summary>
        /// Return collection of posts belongs to a particular PostCategory.
        /// </summary>
        /// <param name="categorySlug">PostCategory's url slug</param>
        /// <param name="pageNo">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public List<Post> GetPostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            var posts = _db.Posts
                            .Where(p => p.Published.HasValue && p.Published.Value > oldDate && p.Categories.Select(c => c.UrlSlug).Contains(categorySlug))
                            .OrderByDescending(p => p.PostedOn)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

            return posts;
        }

        /// <summary>
        /// Return collection of posts that matches the search text.
        /// </summary>
        /// <param name="search">search text</param>
        /// <param name="pageNo">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public List<Post> GetPostsForSearch(string search, int pageNo, int pageSize)
        {
            var posts = _db.Posts
                .Where(p => p.Published.HasValue && p.Published.Value > oldDate && (p.Title.Contains(search) || p.Categories.Select(c => c.Name).Contains(search) || p.Tags.Select(c => c.Name).Contains(search)))
                .OrderByDescending(p => p.PostedOn)
                .Skip(pageNo * pageSize)
                .Take(pageSize)
                .ToList();

            return posts;
        }

        /// <summary>
        /// Return total no. of all or published posts.
        /// </summary>
        /// <param name="checkIsPublished">True to count only published posts</param>
        /// <returns></returns>
        public int TotalPosts(bool checkIsPublished = true)
        {
            return _db.Posts.Count(p => checkIsPublished || p.Published.HasValue == true && p.Published.Value > oldDate);
        }

        /// <summary>
        /// Return total no. of posts belongs to a particular category.
        /// </summary>
        /// <param name="categorySlug">PostCategory's url slug</param>
        /// <returns></returns>
        public int TotalPostsForCategory(string categorySlug)
        {
            return _db.Posts.Count(p => p.Categories.Select(c => c.UrlSlug).Contains(categorySlug) && p.Published.HasValue == true && p.Published.Value > oldDate);
        }

        /// <summary>
        /// Return total no. of posts belongs to a particular tag.
        /// </summary>
        /// <param name="tagSlug">Tag's url slug</param>
        /// <returns></returns>
        public int TotalPostsForTag(string tagSlug)
        {
            return _db.Posts
                      .Where(p => p.Published.HasValue && p.Published.Value > oldDate && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                      .Count();
        }

        /// <summary>
        /// Return total no. of posts that matches the search text.
        /// </summary>
        /// <param name="search">search text</param>
        /// <returns></returns>
        public int TotalPostsForSearch(string search)
        {
            return _db.Posts
                    .Where(p => p.Published.HasValue && p.Published.Value > oldDate && (p.Title.Contains(search) || p.Categories.Any(c => c.Name.Equals(search)) || p.Tags.Any(t => t.Name.Equals(search))))
                    .Count();
        }

        /// <summary>
        /// Return posts based on pagination and sorting parameters.
        /// </summary>
        /// <param name="pageNo">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sortColumn">Sort column name</param>
        /// <param name="sortByAscending">True to sort by ascending</param>
        /// <returns></returns>
        public List<Post> GetPosts(int pageNo, int pageSize, string sortColumn, bool sortByAscending)
        {
            List<Post> posts;

            switch (sortColumn)
            {
                case "Title":
                    if (sortByAscending)
                    {
                        posts = _db.Posts
                            .OrderBy(p => p.Title)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

                    }
                    else
                    {
                        posts = _db.Posts
                            .OrderByDescending(p => p.Title)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

                    }
                    break;
                case "Published":
                    if (sortByAscending)
                    {
                        posts = _db.Posts
                            .OrderBy(p => p.Published)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

                    }
                    else
                    {
                        posts = _db.Posts
                            .OrderByDescending(p => p.Published)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

                    }
                    break;
                case "PostedOn":
                    if (sortByAscending)
                    {
                        posts = _db.Posts
                            .OrderBy(p => p.PostedOn)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();
                    }
                    else
                    {
                        posts = _db.Posts
                            .OrderByDescending(p => p.PostedOn)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

                    }
                    break;
                case "Modified":
                    if (sortByAscending)
                    {
                        posts = _db.Posts
                            .OrderBy(p => p.Modified)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

                    }
                    else
                    {
                        posts = _db.Posts
                            .OrderByDescending(p => p.Modified)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

                    }
                    break;
                case "Category":
                    if (sortByAscending)
                    {
                        posts = _db.Posts
                            .OrderBy(p => p.Categories.FirstOrDefault().Name)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

                    }
                    else
                    {
                        posts = _db.Posts
                            .OrderByDescending(p => p.Categories.FirstOrDefault().Name)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();
                    }
                    break;
                default:
                    posts = _db.Posts
                        .OrderByDescending(p => p.PostedOn)
                        .Skip(pageNo * pageSize)
                        .Take(pageSize)
                        .ToList();

                    break;
            }

            return posts;
        }

        /// <summary>
        /// Return post based on the published year, month and title slug.
        /// </summary>
        /// <param name="year">Published year</param>
        /// <param name="month">Published month</param>
        /// <param name="titleSlug">Post's url slug</param>
        /// <returns></returns>
        public Post GetPost(int year, int month, string titleSlug)
        {
            /*return _db.Query<Post>()
                           .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug))
                           .Fetch(p => p.Category)
                           .FetchMany(p => p.Tags)
                           .FirstOrDefault();*/

            var query = _db.Posts.FirstOrDefault(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug));

            return query;
        }

        /// <summary>
        /// Return post based on unique id, and the number of records after it for a total of 'count' records.
        /// </summary>
        /// <param name="id">Post unique id</param>
        /// <returns></returns>
        public List<Post> GetPostsFromId(int id, int count)
        {

            return _db.Posts.OrderBy(p1 => p1.Id).Where(p2 => p2.Id >= id).Take(10).ToList();
        }

        /// <summary>
        /// Return post based on unique id.
        /// </summary>
        /// <param name="id">Post unique id</param>
        /// <returns></returns>
        public Post GetPost(int? id)
        {
            return _db.Posts.Find(id);
        }

        /// <summary>
        /// Adds a new post and returns the id.
        /// </summary>
        /// <param name="post"></param>
        /// <returns>Newly added post id</returns>
        public int AddPost(Post post)
        {
            _db.Posts.Add(post);
            _db.SaveChanges();
            return post.Id;
        }

        /// <summary>
        /// Update an existing post.
        /// </summary>
        /// <param name="post"></param>
        public void UpdatePost(Post post)
        {
            var item = _db.Posts.Find(post.Id);
            item.Approved = post.Approved;
            item.Body = post.Body;
            item.Categories = post.Categories;
            item.Meta = post.Meta;
            item.Approved = post.Approved;
            item.Modified = post.Modified;
            item.PostedOn = post.PostedOn;
            item.Published = post.Published;
            item.Subject = post.Subject;
            item.Tags = post.Tags;
            item.Title = post.Title;
            item.UrlSlug = post.UrlSlug;

            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        /// <summary>
        /// Delete the post permanently from database.
        /// </summary>
        /// <param name="id"></param>
        public void DeletePost(int id)
        {
            var item = _db.Posts.Find(id);
            _db.Posts.Remove(item);
            _db.SaveChanges();
        }

        /// <summary>
        /// Return all the categories.
        /// </summary>
        /// <returns></returns>
        public List<PostCategory> GetCategories()
        {
            return _db.PostCategories.OrderBy(p => p.Name).ToList();
        }

        /// <summary>
        /// Return total no. of categories.
        /// </summary>
        /// <returns></returns>
        public int TotalCategories()
        {
            return _db.PostCategories.Count();
        }

        /// <summary>
        /// Return category based on url slug.
        /// </summary>
        /// <param name="categorySlug">Category's url slug</param>
        /// <returns></returns>
        public PostCategory GetCategory(string categorySlug)
        {
            return _db.PostCategories.FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }

        /// <summary>
        /// Return category based on id.
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        public PostCategory GetCategory(int id)
        {
            return _db.PostCategories.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int AddCategory(PostCategory category)
        {
            _db.PostCategories.Add(category);
            _db.SaveChanges();
            return category.Id;
        }

        /// <summary>
        /// Update an existing category.
        /// </summary>
        /// <param name="category"></param>
        public void UpdateCategory(PostCategory category)
        {
            var item = _db.PostCategories.Find(category.Id);
            item.Description = category.Description;
            item.Name = category.Name;
            item.Posts = category.Posts;
            item.UrlSlug = category.UrlSlug;

            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        /// <summary>
        /// Delete a category permanently.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCategory(int id)
        {
            var item = _db.PostCategories.Find(id);
            _db.PostCategories.Remove(item);
            _db.SaveChanges();
        }

        /// <summary>
        /// Return all the tags.
        /// </summary>
        /// <returns></returns>
        public List<PostTag> GetTags()
        {
            return _db.PostTags.OrderBy(p => p.Name).ToList();
        }

        /// <summary>
        /// Return total no. of tags.
        /// </summary>
        /// <returns></returns>
        public int TotalTags()
        {
            return _db.PostTags.Count();
        }

        /// <summary>
        /// Return tag based on url slug.
        /// </summary>
        /// <param name="tagSlug"></param>
        /// <returns></returns>
        public PostTag GetTag(string tagSlug)
        {
            return _db.PostTags.FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));
        }

        /// <summary>
        /// Return tag based on unique id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PostTag GetTag(int id)
        {
            return _db.PostTags.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Add a new tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int AddTag(PostTag tag)
        {
            _db.PostTags.Add(tag);
            _db.SaveChanges();
            return tag.Id;
        }

        /// <summary>
        /// Update an existing tag.
        /// </summary>
        /// <param name="tag"></param>
        public void UpdateTag(PostTag tag)
        {
            var item = _db.PostTags.Find(tag.Id);
            item.Description = tag.Description;
            item.Name = tag.Name;
            item.Posts = tag.Posts;
            item.UrlSlug = tag.UrlSlug;

            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
        }

        /// <summary>
        /// Delete an existing tag permanently.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteTag(int id)
        {
            var item = _db.PostTags.Find(id);
            _db.PostTags.Remove(item);
            _db.SaveChanges();
        }
        #endregion


    }
}
