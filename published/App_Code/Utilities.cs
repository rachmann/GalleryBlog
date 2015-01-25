using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CraftStore.Common
{
    public static class Utilities
    {
        public static string ToAbsoluteUrl(HttpContextBase httpContext, string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (httpContext == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = httpContext.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        public static void Log(string msg)
        {
            var filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "log\\" + "logging.txt";
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }
            var sw = new System.IO.StreamWriter(filename, true);
            sw.WriteLine(DateTime.Now.ToLongTimeString() + " : " + msg);
            sw.Close();
        }

        public static string ToAbsoluteUrl(System.Web.HttpContext httpContext, string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (httpContext == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = httpContext.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

    }
}