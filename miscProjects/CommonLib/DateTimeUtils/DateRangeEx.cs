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
    }
}
