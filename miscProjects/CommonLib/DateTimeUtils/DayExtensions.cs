using System;

namespace CommonLib.DateUtils
{
    /// <summary>
    /// DayExtensions
    /// </summary>
    public static class DayExtensions
    {
        /// <summary>
        /// EstToUtc
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EstToUtc(this DateTime dt)
        {
            return dt.EstToUtcDto().DateTime;
        }

        /// <summary>
        /// EstToLocal
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EstToLocal(this DateTime dt)
        {
            DateTime dtEst = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            DateTime dtUtc = dtEst.EstToUtc();
            DateTime dtEventTimeLocal = dtUtc.ToLocalTime();
            return dtEventTimeLocal;
        }

        /// <summary>
        /// EstToUtcDto
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTimeOffset EstToUtcDto(this DateTime dt)
        {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTimeOffset dtoGMT = new DateTimeOffset(dt, tzi.BaseUtcOffset).UtcDateTime;
            TimeSpan tsbase = TimeZoneInfo.Local.BaseUtcOffset;
            TimeSpan ts = TimeZoneInfo.Local.GetUtcOffset(dtoGMT.LocalDateTime);
            return dtoGMT;
        }

        /// <summary>
        /// Returns first next occurence of specified DayOfTheWeek
        /// </summary>
        /// <param name="start"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime Previous(this DateTime start, DayOfWeek day)
        {
            do
            {
                start = start.PreviousDay();
            }
            while (start.DayOfWeek != day);

            return start;
        }
        /// <summary>
        /// Returns DateTime decreased by 24h period ie Previous Day
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static DateTime PreviousDay(this DateTime start)
        {
            return start - 1.Days();
        }
        /// <summary>
        /// Returns TimeSpan for given number of Days
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static TimeSpan Days(this int days)
        {
            return new TimeSpan(days, 0, 0, 0, 0);
        }

        /// <summary>
        /// Gets a DateTime representing the first day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current)
        {
            DateTime first = current.AddDays(1 - current.Day);
            return first;
        }


        /// <summary>
        /// Gets a DateTime representing the first specified day in the current month
        /// </summary>
        /// <param name="current">The current day</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime first = current.First();

            if (first.DayOfWeek != dayOfWeek)
            {
                first = first.Next(dayOfWeek);
            }

            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the last day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current)
        {
            int daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);

            DateTime last = current.First().AddDays(daysInMonth - 1);
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the last specified day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime last = current.Last();

            last = last.AddDays(Math.Abs(dayOfWeek - last.DayOfWeek) * -1);
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the first date following the current date which falls on the given day of the week
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The day of week for the next date to get</param>
        public static DateTime Next(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;

            if (offsetDays <= 0)
            {
                offsetDays += 7;
            }

            DateTime result = current.AddDays(offsetDays);
            return result;
        }


    }
}
