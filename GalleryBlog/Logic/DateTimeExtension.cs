using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GalleryBlog
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Convert the passed datetime from UTC timezone to configured timezone in web.config.
        /// </summary>
        /// <param name="utcDt"></param>
        /// <returns></returns>
        public static string ToConfigLocalTime(this DateTime utcDt)
        {
            int hourOffset = -5;
            if(!int.TryParse( ConfigurationManager.AppSettings["TimezoneOffset"], out hourOffset))
            {
                hourOffset = -5;
            }
            // just ensure that it is actually UTC.
            var localDate = utcDt.ToUniversalTime();
            localDate = localDate.AddHours(hourOffset);
            //var istTZ = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["Timezone"]);
            var istTZ = TimeZoneInfo.Local;
            return String.Format("{0} ({1})", localDate.ToString("dd MMM yyyy"), ConfigurationManager.AppSettings["TimezoneAbbr"]);
        }

    }
}