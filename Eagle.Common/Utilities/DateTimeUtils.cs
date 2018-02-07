using System;
using System.Globalization;
using System.Net;

namespace Eagle.Common.Utilities
{
    public static class DateTimeUtils
    {
        public static DateTime GetCurrentDate()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetCurrentDateUtc()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Returns a platform-specific DateTime.UtcNow
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime PlatformNow(this DateTime dateTime)
        {
            return DateTime.UtcNow;
        }
        public static string DDD_DD_MMM(this DateTime d)
        {

            return d.ToString("ddd dd MMM", CultureInfo.CurrentCulture);
        }
        public static string TimeOfDay(this DateTime d)
        {

            return d.ToString("h:mm tt", CultureInfo.CurrentCulture).ToLower();
        }

        public static string ToICalDate(this DateTime d)
        {
            return d.ToString("O");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DateTimeToString(DateTime? info, string format)
        {
            try
            {
                if (info.HasValue)
                {
                    return info.Value.ToString(format);
                }
                return String.Empty;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return String.Empty;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string info, string format)
        {
            return DateTime.ParseExact(info, format, new DateTimeFormatInfo());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToString(DateTime info, string format)
        {
            try
            {
                return info.ToString(format);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                return String.Empty;
            }
        }
        public static string[] DatetimeFormats = {
                           "M/d/yyyy", "M/d/yyyy h:mm:ss","M/d/yyyy h:mm:ss tt","M/d/yyyy h:mm tt","M/d/yyyy hh:mm tt","M/d/yyyy hh tt","M/d/yyyy h:mm","M/d/yy",
                           "MM/dd/yyyy", "MM/dd/yyyy hh:mm:ss", "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm",
                           "dd/MM/yyyy","dd-MMM-yy","dd-MMM-yyyy","dd/MM/yyyy hh:mm:ss tt", "dd/MM/yyyy hh:mm:ss","dd/MM/yyyy hh:mm",
                           "yy/MM/dd","yyyy/MM/dd","yyyy/MM/dd hh:mm:ss tt","yyyy-MM-dd","yyyy-MM-dd HH:mm:ss"};

        //public static string[] datetime_formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt", 
        //           "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss", 
        //           "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt", 
        //           "M/d/yyyy h:mm", "M/d/yyyy h:mm", 
        //           "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};

        //public static string FormatDateTimeString(string strInput, string strCustomFormat)
        //{
        //    string strFormattedDateTime = string.Empty;
        //    if(strInput.Length >=8)
        //    {
        //        DateTime SELECTED_DATE;
        //        if (DateTime.TryParseExact(strInput, datetime_formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out SELECTED_DATE))
        //            strFormattedDateTime = SELECTED_DATE.ToString(strCustomFormat);
        //        }                    
        //        else
        //        {
        //            SELECTED_DATE = Convert.ToDateTime(strInput);
        //            strFormattedDateTime = SELECTED_DATE.ToString(strCustomFormat);
        //        }   
        //    }
        //    return strFormattedDateTime;
        //}

        public static string FormatDateTimeString(string strInput, string strCustomFormat)
        {
            string strFormattedDateTime = string.Empty;
            if (strInput.Length >= 8)
            {
                DateTime selectedDate;
                if (DateTime.TryParseExact(strInput, strCustomFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate))
                    strFormattedDateTime = selectedDate.ToString(strCustomFormat);
                else
                {
                    selectedDate = Convert.ToDateTime(strInput);
                    strFormattedDateTime = selectedDate.ToString(strCustomFormat);
                }
            }
            return strFormattedDateTime;
        }
        public static DateTime ParseDate(string strDateTimeInput)
        {
            DateTime formattedDateTime = new DateTime();
            if (strDateTimeInput.Length >= 8)
            {
                DateTime selectedDate;
                if (DateTime.TryParseExact(strDateTimeInput, DatetimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDate))
                    formattedDateTime = selectedDate;
                else
                    formattedDateTime = Convert.ToDateTime(strDateTimeInput);
            }
            return formattedDateTime;
        }

        /// <summary>
        /// Parse DateTime To DimDate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? ConvertDateTimeToDimDate(DateTime obj)
        {
            try
            {
                int? result = null;
                if (IsDateTime(obj))
                {
                    result = Convert.ToInt32(DateTime.Parse(obj.ToString(CultureInfo.InvariantCulture)).ToString("yyyyMMdd"));
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DateTime? ConvertDimDateToDateTime(object obj)
        {
            try
            {
                DateTime? result = null;
                if (ValidatorUtils.IsInt(obj))
                {
                    string strDateTime = obj.ToString();
                    result = new DateTime(ValidatorUtils.ParseInt(strDateTime.Substring(0, 4)), ValidatorUtils.ParseInt(strDateTime.Substring(4, 2)), ValidatorUtils.ParseInt(strDateTime.Substring(6, 2)));
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        /// <summary>
        /// Convert datetime to Unix timestamp
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        public static bool TryConvertToDate(string inputDate, out DateTime? outputDate)
        {

            try
            {
                string[] arrayDate = inputDate.Split('/');
                outputDate = new DateTime(int.Parse(arrayDate[2]), int.Parse(arrayDate[1]), int.Parse(arrayDate[0]));
                return true;
            }
            catch// (Exception ex)
            {
                outputDate = null;
                return false;
            }
        }
        public static double ConvertToTimestampDouble(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

            //return the total seconds (which is a UNIX timestamp)
            return span.TotalSeconds;

        }
        public static string ConvertToTimestampString(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

            //return the total seconds (which is a UNIX timestamp)
            return span.TotalSeconds.ToString(CultureInfo.InvariantCulture);

        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime UnixTimeStampToDateTime(string unixTimeStampString)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(double.Parse(unixTimeStampString)).ToLocalTime();
            return dtDateTime;
        }
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
        
        #region Validate DateTime ====================================================
        public static bool IsValidDateTime(string val, string format = null)
        {
            if (string.IsNullOrEmpty(format))
                format = "yyyy-MM-dd HH:mm:ss";

            DateTime parsedDateTime;
            return DateTime.TryParseExact(val, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeInfo"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static bool IsDateTime(string dateTimeInfo, string format)
        {
            if (!string.IsNullOrEmpty(dateTimeInfo) && !string.IsNullOrEmpty(format))
            {
                DateTime.ParseExact(dateTimeInfo, format, new DateTimeFormatInfo());
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validate DateTime Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDateTime(object obj)
        {
            try
            {
                DateTime.Parse(obj.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
      
        #endregion Validate DateTime =================================================


        public static int FirstDayOfPredefinedPreviousMonth(int predefinedPreviousMonth)
        {
            DateTime firstDayOfPredefinedPreviousMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-Math.Abs(predefinedPreviousMonth));
            return firstDayOfPredefinedPreviousMonth.Day;
        }

        public static int LastDayOfPredefinedPreviousMonth(int predefinedPreviousMonth)
        {
            DateTime lastDayOfPreviousMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month - predefinedPreviousMonth, 1).AddDays(-1);
            return lastDayOfPreviousMonth.Day;
        }

        public static DateTime FirstDateOfPredefinedPreviousMonth(int predefinedPreviousMonth)
        {
            DateTime firstDateOfPredefinedPreviousMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-Math.Abs(predefinedPreviousMonth));
            return firstDateOfPredefinedPreviousMonth;
        }

        public static DateTime LastDateOfPredefinedPreviousMonth(int predefinedPreviousMonth)
        {
            DateTime lastDateOfPredefinedPreviousMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-Math.Abs(predefinedPreviousMonth));
            return lastDateOfPredefinedPreviousMonth;
        }

        public static int FirstDayOfPreviousMonth()
        {
            DateTime firstDayOfPreviousMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            return firstDayOfPreviousMonth.Day;
        }

        public static DateTime FirstDateOfPreviousMonth()
        {
            DateTime firstDayOfPreviousMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            return firstDayOfPreviousMonth;
        }

        public static int LastDayOfPreviousMonth()
        {
            DateTime lastDayOfPreviousMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
            return lastDayOfPreviousMonth.Day;
        }

        public static DateTime LastDateOfPreviousMonth()
        {
            DateTime lastDateOfPreviousMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
            return lastDateOfPreviousMonth;
        }

        public static int DaysInMonthOfSelectedDate(DateTime dateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return daysInMonth;
        }

        public static int FirstDayOfCurrentMonth()
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return firstDayOfMonth.Day;
        }

        public static DateTime FirstDateOfCurrentMonth()
        {
            DateTime firstDateOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return firstDateOfMonth;
        }

        public static int LastDayOfCurrentMonth()
        {
            DateTime firstDayOfCurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime lastDayOfCurrentMonth = firstDayOfCurrentMonth.AddMonths(1).AddSeconds(-1);
            return lastDayOfCurrentMonth.Day;
        }

        public static DateTime LastDateOfCurrentMonth()
        {
            DateTime firstDateOfCurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime lastDateOfCurrentMonth = firstDateOfCurrentMonth.AddMonths(1).AddSeconds(-1);
            return lastDateOfCurrentMonth;
        }

        public static int FirstDayOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(1 - (int)DateTime.UtcNow.DayOfWeek).Day;
        }

        public static DateTime FirstDateOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(1 - (int)DateTime.UtcNow.DayOfWeek);
        }

        public static int LastDayOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(7 - (int)DateTime.UtcNow.DayOfWeek).Day;
        }

        public static DateTime LastDateOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(7 - (int)DateTime.UtcNow.DayOfWeek);
        }

        public static DateTime TuesdayOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(2 - (int)DateTime.UtcNow.DayOfWeek);
        }

        public static DateTime WednesdayOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(3 - (int)DateTime.UtcNow.DayOfWeek);
        }

        public static DateTime ThursdayOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(4 - (int)DateTime.UtcNow.DayOfWeek);
        }

        public static DateTime LastFridayOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(5 - (int)DateTime.UtcNow.DayOfWeek);
        }

        public static DateTime LastSarturdayOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(6 - (int)DateTime.UtcNow.DayOfWeek);
        }

        public static DateTime LastSundayOfCurrentWeek()
        {
            return DateTime.UtcNow.AddDays(7 - (int)DateTime.UtcNow.DayOfWeek);
        }


        //public static DateTime GetNistTime()
        //{
        //    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
        //    var response = myHttpWebRequest.GetResponse();
        //    string todaysDates = response.Headers["date"];
        //    return DateTime.ParseExact(todaysDates,
        //        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
        //        CultureInfo.InvariantCulture.DateTimeFormat,
        //        DateTimeStyles.AssumeUniversal);
        //}
    }
}
