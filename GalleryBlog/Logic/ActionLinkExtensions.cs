
using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GalleryBlog.Models;
using System.Collections.Generic;

namespace GalleryBlog
{
  public static class ActionLinkExtensions
  {
    public static MvcHtmlString PostLink(this HtmlHelper helper, Post post)
    {
      return helper.ActionLink(post.Title, "Post", "Home", new { year = post.PostedOn.Year, month = post.PostedOn.Month, title = post.UrlSlug }, new { title = post.Title });
    }

    public static MvcHtmlString PostCategoryLink(this HtmlHelper helper, PostCategory category)
    {
        if (category == null)
        {
            return MvcHtmlString.Empty; 
        }
        else
        {
            return helper.ActionLink(category.Name, "Category", "Home", new { category = category.UrlSlug }, new { title = String.Format("See all posts in {0}", category.Name) });
        }
    }

    public static MvcHtmlString TagLink(this HtmlHelper helper, PostTag tag)
    {
        if (tag == null)
        {
            return MvcHtmlString.Empty; 
        }
        else
        {
            return helper.ActionLink(tag.Name, "Tag", "Home", new { tag = tag.UrlSlug }, new { title = String.Format("See all posts in {0}", tag.Name) });
        }
    }
  }
}