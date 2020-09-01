using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.DateUtils
{
    /// <summary>
    /// DateRange Extensions
    /// </summary>
    public static class DateRangeEx
    {
        /// <summary>
        /// Get Weeks
        /// </summary>
        /// <param name="recs"></param>
        /// <param name="startDate"></param>
        /// <param name="totalWeeks"></param>
        /// <returns></returns>
        public static List<DateRange> Weeks(this List<DateRange> recs, DateTime startDate, int totalWeeks)
        {
            recs = new List<DateRange>();
            if (startDate.DayOfWeek != DayOfWeek.Sunday)
                startDate = startDate.Previous(DayOfWeek.Sunday);

            DateTime endDate = startDate.AddDays((totalWeeks * 7));
            while (startDate <= endDate)
            {
                recs.Add(new DateRange(startDate, startDate.Next(DayOfWeek.Friday)));
                startDate = startDate.Next(DayOfWeek.Sunday);
            }
            return recs;
        }

        public static List<DateRange> Weeks(this List<DateRange> recs, DateTime startDate)
        {
            return recs.Weeks(startDate, DateTime.Today);
        }

        public static List<DateRange> Weeks(this List<DateRange> recs, DateTime startDate, DateTime endDate)
        {
            recs = new List<DateRange>();
            if (startDate.DayOfWeek != DayOfWeek.Sunday)
                startDate = startDate.Previous(DayOfWeek.Sunday);

            while (startDate <= endDate)
            {
                recs.Add(new DateRange(startDate, startDate.Next(DayOfWeek.Friday)));
                startDate = startDate.Next(DayOfWeek.Sunday);
            }
            return recs;
        }

        public static DateRange ThisWeek(this DateRange dr)
        {
            var dt = DateTime.Now;
            dr = new DateRange(dt.Previous(DayOfWeek.Sunday), dt.Next(DayOfWeek.Friday));
            return dr;
        }

        public static DateRange ThisWeek(this DateTime dt)
        {
            dt = DateTime.Now;
            var dr = new DateRange(dt.Previous(DayOfWeek.Sunday), dt.Next(DayOfWeek.Friday));
            return dr;
        }

        public static DateRange PreviousWeek(this DateTime dt)
        {
            dt = DateTime.Now.AddDays(-7);
            var dr = new DateRange(dt.Previous(DayOfWeek.Sunday), dt.Next(DayOfWeek.Friday));
            return dr;
        }

        public static DateRange WeekOf(this DateTime startDate)
        {
            if (startDate.DayOfWeek == DayOfWeek.Sunday)
                return new DateRange(startDate, startDate.Next(DayOfWeek.Friday));

            if (startDate.DayOfWeek == DayOfWeek.Saturday)
                return new DateRange(startDate.Previous(DayOfWeek.Sunday), startDate.Previous(DayOfWeek.Friday));

            return new DateRange(startDate.Previous(DayOfWeek.Sunday), startDate.Next(DayOfWeek.Friday));
        }
    }
}


