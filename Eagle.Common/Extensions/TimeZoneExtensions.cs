using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Eagle.Common.Extensions
{
    public class TimeZoneExtensions
    {
        public static SelectList PoplulateTimeZoneSelectList(string selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = TimeZoneInfo.GetSystemTimeZones();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.DisplayName, Value = p.Id }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = "--- SelectTimeZone ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- None --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
        public static TimeZoneInfo TimeZoneToTimeZoneInfo(string tzTimeZoneId)
        {
            //tz to Windows time zone mapping can be found in http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/zone_tzid.html
            var windowsTimeZone = default(TimeZoneInfo);
            try
            {
                var firstOrDefault = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.Id == tzTimeZoneId);
                if (firstOrDefault != null)
                {
                    string windowsTimeZoneId = firstOrDefault.Id;
                    windowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId);
                }
            }catch (TimeZoneNotFoundException) { }
            catch (InvalidTimeZoneException) { }
            return windowsTimeZone;
        }

        public static double ToTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        /// <summary>
        /// Convert Utc DateTime to timezone time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime FromUtcToTimeZoneTime(DateTime date, string timeZoneId)
        {
            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(date, tzInfo);
        }

        /// <summary>
        /// Convert Utc DateTime to timezone time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static DateTime FromUtcToTimeZoneTime(DateTime date, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date, timeZone);
        }

        /// <summary>
        /// Convert Timezone DateTime to utc time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime FromTimeZoneToUtcTime(DateTime date, string timeZoneId)
        {
            var tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(date, tzInfo);
        }

        /// <summary>
        /// Convert Timezone DateTime to utc time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static DateTime FromTimeZoneToUtcTime(DateTime date, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(date, timeZone);
        }

        public static string GetTimeZoneOffset(string timezoneId)
        {
            TimeZoneInfo tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);

            if (tzInfo.BaseUtcOffset.Hours > 0)
                return $"+{tzInfo.BaseUtcOffset:hh\\:mm}";
            else if (tzInfo.BaseUtcOffset.Hours < 0)
                return $"-{tzInfo.BaseUtcOffset:hh\\:mm}";
            else
                return $"{tzInfo.BaseUtcOffset:hh\\:mm}";
        }
    }
}
