using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryBlog.Models
{
    public class DataAccess
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        #region Post Tag Logic

        /// <summary>
        /// Return collection of posts based on pagination parameters.
        /// </summary>
        /// <param name="pageNo">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public IList<Post> GetPosts(int pageNo, int pageSize)
        {

            var posts = _db.Posts.Where(p => p.Published.HasValue)
                                  .OrderByDescending(p => p.PostedOn)
                                  .Skip(pageNo * pageSize)
                                  .Take(pageSize).ToList();

        }

        /// <summary>
        /// Return collection of posts belongs to a particular tag.
        /// </summary>
        /// <param name="tagSlug">Tag's url slug</param>
        /// <param name="pageNo">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public IList<Post> GetPostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            var posts = _db.Posts
                            .Where(p => p.Published.HasValue && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                            .OrderByDescending(p => p.PostedOn)
                            .Skip(pageNo * pageSize)
                            .Take(pageSize)
                            .ToList();

        }

        /// <summary>
        /// Return collection of posts belongs to a particular category.
        /// </summary>
        /// <param name="categorySlug">Category's url slug</param>
        /// <param name="pageNo">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public IList<Post> GetPostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            var posts = _db.Posts
                            .Where(p => p.Published.HasValue && p.Categories.Select(c=>c.UrlSlug).Contains(categorySlug))
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
        public IList<Post> GetPostsForSearch(string search, int pageNo, int pageSize)
        {
            var posts = _db.Posts
                .Where(p => p.Published.HasValue && (  p.Title.Contains(search) || p.Categories.Select(c => c.Name).Contains(search) || p.Tags.Select(c => c.Name).Contains(search)))
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
            return _db.Posts.Count(p => checkIsPublished || p.Published.HasValue == true);
        }

        /// <summary>
        /// Return total no. of posts belongs to a particular category.
        /// </summary>
        /// <param name="categorySlug">Category's url slug</param>
        /// <returns></returns>
        public int TotalPostsForCategory(string categorySlug)
        {
            return _db.Posts.Count(p => p.Categories.Select(c => c.UrlSlug).Contains(categorySlug) && p.Published.HasValue == true);
        }

        /// <summary>
        /// Return total no. of posts belongs to a particular tag.
        /// </summary>
        /// <param name="tagSlug">Tag's url slug</param>
        /// <returns></returns>
        public int TotalPostsForTag(string tagSlug)
        {
            return _session.Query<Post>()
                           .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                           .Count();
        }

        /// <summary>
        /// Return total no. of posts that matches the search text.
        /// </summary>
        /// <param name="search">search text</param>
        /// <returns></returns>
        public int TotalPostsForSearch(string search)
        {
            return _session.Query<Post>()
                           .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
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
        public IList<Post> GetPosts(int pageNo, int pageSize, string sortColumn, bool sortByAscending)
        {
            IList<Post> posts;
            IList<int> postIds;

            switch (sortColumn)
            {
                case "Title":
                    if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                        .OrderBy(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                         .Where(p => postIds.Contains(p.Id))
                                         .OrderBy(p => p.Title)
                                         .FetchMany(p => p.Tags)
                                         .ToList();
                    }
                    else
                    {
                        posts = _session.Query<Post>()
                                        .OrderByDescending(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                         .Where(p => postIds.Contains(p.Id))
                                         .OrderByDescending(p => p.Title)
                                         .FetchMany(p => p.Tags)
                                         .ToList();
                    }
                    break;
                case "Published":
                    if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                        .OrderBy(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                         .Where(p => postIds.Contains(p.Id))
                                         .OrderBy(p => p.Published)
                                         .FetchMany(p => p.Tags)
                                         .ToList();
                    }
                    else
                    {
                        posts = _session.Query<Post>()
                                        .OrderByDescending(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                         .Where(p => postIds.Contains(p.Id))
                                         .OrderByDescending(p => p.Published)
                                         .FetchMany(p => p.Tags)
                                         .ToList();
                    }
                    break;
                case "PostedOn":
                    if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                        .OrderBy(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                         .Where(p => postIds.Contains(p.Id))
                                         .OrderBy(p => p.PostedOn)
                                         .FetchMany(p => p.Tags)
                                         .ToList();
                    }
                    else
                    {
                        posts = _session.Query<Post>()
                                        .OrderByDescending(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                        .Where(p => postIds.Contains(p.Id))
                                        .OrderByDescending(p => p.PostedOn)
                                        .FetchMany(p => p.Tags)
                                        .ToList();
                    }
                    break;
                case "Modified":
                    if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                        .OrderBy(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                         .Where(p => postIds.Contains(p.Id))
                                         .OrderBy(p => p.Modified)
                                         .FetchMany(p => p.Tags)
                                         .ToList();
                    }
                    else
                    {
                        posts = _session.Query<Post>()
                                        .OrderByDescending(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                         .Where(p => postIds.Contains(p.Id))
                                         .OrderByDescending(p => p.Modified)
                                         .FetchMany(p => p.Tags)
                                         .ToList();
                    }
                    break;
                case "Category":
                    if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                        .OrderBy(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                         .Where(p => postIds.Contains(p.Id))
                                         .OrderBy(p => p.Category.Name)
                                         .FetchMany(p => p.Tags)
                                         .ToList();
                    }
                    else
                    {
                        posts = _session.Query<Post>()
                                        .OrderByDescending(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize)
                                        .Fetch(p => p.Category)
                                        .ToList();

                        postIds = posts.Select(p => p.Id).ToList();

                        posts = _session.Query<Post>()
                                         .Where(p => postIds.Contains(p.Id))
                                         .OrderByDescending(p => p.Category.Name)
                                         .FetchMany(p => p.Tags)
                                         .ToList();
                    }
                    break;
                default:
                    posts = _session.Query<Post>()
                                    .OrderByDescending(p => p.PostedOn)
                                    .Skip(pageNo * pageSize)
                                    .Take(pageSize)
                                    .Fetch(p => p.Category)
                                    .ToList();

                    postIds = posts.Select(p => p.Id).ToList();

                    posts = _session.Query<Post>()
                                     .Where(p => postIds.Contains(p.Id))
                                     .OrderByDescending(p => p.PostedOn)
                                     .FetchMany(p => p.Tags)
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
            /*return _session.Query<Post>()
                           .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug))
                           .Fetch(p => p.Category)
                           .FetchMany(p => p.Tags)
                           .FirstOrDefault();*/

            var query = _db.Posts.FirstOrDefault(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug));

            return query;
        }

        /// <summary>
        /// Return post based on unique id.
        /// </summary>
        /// <param name="id">Post unique id</param>
        /// <returns></returns>
        public Post GetPost(int id)
        {
            return _session.Get<Post>(id);
        }

        /// <summary>
        /// Adds a new post and returns the id.
        /// </summary>
        /// <param name="post"></param>
        /// <returns>Newly added post id</returns>
        public int AddPost(Post post)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.Save(post);
                tran.Commit();
                return post.Id;
            }
        }

        /// <summary>
        /// Update an existing post.
        /// </summary>
        /// <param name="post"></param>
        public void EditPost(Post post)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(post);
                tran.Commit();
            }
        }

        /// <summary>
        /// Delete the post permanently from database.
        /// </summary>
        /// <param name="id"></param>
        public void DeletePost(int id)
        {
            using (var tran = _session.BeginTransaction())
            {
                var post = _session.Get<Post>(id);
                if (post != null) _session.Delete(post);
                tran.Commit();
            }
        }

        /// <summary>
        /// Return all the categories.
        /// </summary>
        /// <returns></returns>
        public IList<Category> GetCategories()
        {
            return _session.Query<Category>().OrderBy(p => p.Name).ToList();
        }

        /// <summary>
        /// Return total no. of categories.
        /// </summary>
        /// <returns></returns>
        public int TotalCategories()
        {
            return _session.Query<Category>().Count();
        }

        /// <summary>
        /// Return category based on url slug.
        /// </summary>
        /// <param name="categorySlug">Category's url slug</param>
        /// <returns></returns>
        public Category GetCategory(string categorySlug)
        {
            return _session.Query<Category>().FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }

        /// <summary>
        /// Return category based on id.
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        public Category GetCategory(int id)
        {
            return _session.Query<Category>().FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int AddCategory(Category category)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.Save(category);
                tran.Commit();
                return category.Id;
            }
        }

        /// <summary>
        /// Update an existing category.
        /// </summary>
        /// <param name="category"></param>
        public void EditCategory(Category category)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(category);
                tran.Commit();
            }
        }

        /// <summary>
        /// Delete a category permanently.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCategory(int id)
        {
            using (var tran = _session.BeginTransaction())
            {
                var category = _session.Get<Category>(id);
                _session.Delete(category);
                tran.Commit();
            }
        }

        /// <summary>
        /// Return all the tags.
        /// </summary>
        /// <returns></returns>
        public IList<Tag> GetTags()
        {
            return _session.Query<Tag>().OrderBy(p => p.Name).ToList();
        }

        /// <summary>
        /// Return total no. of tags.
        /// </summary>
        /// <returns></returns>
        public int TotalTags()
        {
            return _session.Query<Tag>().Count();
        }

        /// <summary>
        /// Return tag based on url slug.
        /// </summary>
        /// <param name="tagSlug"></param>
        /// <returns></returns>
        public Tag GetTag(string tagSlug)
        {
            return _session.Query<Tag>().FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));
        }

        /// <summary>
        /// Return tag based on unique id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tag GetTag(int id)
        {
            return _session.Query<Tag>().FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Add a new tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int AddTag(Tag tag)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.Save(tag);
                tran.Commit();
                return tag.Id;
            }
        }

        /// <summary>
        /// Edit an existing tag.
        /// </summary>
        /// <param name="tag"></param>
        public void EditTag(Tag tag)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(tag);
                tran.Commit();
            }
        }

        /// <summary>
        /// Delete an existing tag permanently.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteTag(int id)
        {
            using (var tran = _session.BeginTransaction())
            {
                var tag = _session.Get<Tag>(id);
                _session.Delete(tag);
                tran.Commit();
            }
        }
        #endregion

    }
}
