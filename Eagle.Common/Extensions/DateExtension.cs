using System;
using System.Globalization;

namespace Eagle.Common.Extensions
{
    public class DateExtension
    {
        public static long DateDiffFromNow(DateInterval interval, DateTime prevDateTime)
        {
            DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            return DateDiff(interval, prevDateTime, DateTime.UtcNow, firstDayOfWeek);
        }

        public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2)
        {
            DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            return DateDiff(interval, dt1, dt2, firstDayOfWeek);
        }

        /// <summary>
        /// Returns the first day of the week that the specified
        /// date is in using the current culture. 
        /// </summary>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }

        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        private static int GetQuarter(int nMonth)
        {
            if (nMonth <= 3)
                return 1;
            if (nMonth <= 6)
                return 2;
            if (nMonth <= 9)
                return 3;
            return 4;
        }

        public static DateTime DateAdd(DateInterval interval, DateTime dt, Int32 val)
        {
            if (interval == DateInterval.Year)
                return dt.AddYears(val);
            else if (interval == DateInterval.Month)
                return dt.AddMonths(val);
            else if (interval == DateInterval.Day)
                return dt.AddDays(val);
            else if (interval == DateInterval.Hour)
                return dt.AddHours(val);
            else if (interval == DateInterval.Minute)
                return dt.AddMinutes(val);
            else if (interval == DateInterval.Second)
                return dt.AddSeconds(val);
            else if (interval == DateInterval.Quarter)
                return dt.AddMonths(val * 3);
            else
                return dt;
        }

        public static long DateDiff(DateInterval interval, DateTime dt1, DateTime dt2, DayOfWeek eFirstDayOfWeek)
        {
            if (interval == DateInterval.Year)
            {
                return dt2.Year - dt1.Year;
            }

            if (interval == DateInterval.Month)
                return (dt2.Month - dt1.Month) +(12 * (dt2.Year - dt1.Year));

            TimeSpan ts = dt2 - dt1;

            if (interval == DateInterval.Day || interval == DateInterval.DayOfYear)
                return Round(ts.TotalDays);

            if (interval == DateInterval.Hour)
                return Round(ts.TotalHours);

            if (interval == DateInterval.Minute)
                return Round(ts.TotalMinutes);

            if (interval == DateInterval.Second)
                return Round(ts.TotalSeconds);

            if (interval == DateInterval.Weekday)
            {
                return Round(ts.TotalDays / 7.0);
            }

            if (interval == DateInterval.WeekOfYear)
            {
                while (dt2.DayOfWeek != eFirstDayOfWeek)
                    dt2 = dt2.AddDays(-1);
                while (dt1.DayOfWeek != eFirstDayOfWeek)
                    dt1 = dt1.AddDays(-1);
                ts = dt2 - dt1;
                return Round(ts.TotalDays / 7.0);
            }

            if (interval == DateInterval.Quarter)
            {
                double d1Quarter = GetQuarter(dt1.Month);
                double d2Quarter = GetQuarter(dt2.Month);
                double d1 = d2Quarter - d1Quarter;
                double d2 = (4 * (dt2.Year - dt1.Year));
                return Round(d1 + d2);
            }

            return 0;

        }

        private static long Round(double dVal)
        {
            if (dVal >= 0)
                return (long)Math.Floor(dVal);
            return (long)Math.Ceiling(dVal);
        }
    }
}
