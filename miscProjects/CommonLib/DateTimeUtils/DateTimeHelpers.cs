using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.DateUtils
{
    /// <summary>
    /// DateTimeHelpers
    /// </summary>
    public class DateTimeHelpers
    {
        public static DateTime ToTimezone(DateTime input, string to)
        {
            TimeZoneInfo tzTo = TimeZoneInfo.FindSystemTimeZoneById(to);
            DateTime output = TimeZoneInfo.ConvertTimeFromUtc(input.ToUniversalTime(), tzTo);
            return output;
        }
        /// <summary>
        /// Convert to EST
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTimeOffset ConvertToEst(DateTime dt)
        {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            return DateTimeConvertTz(tzi, dt);
        }

        /// <summary>
        /// Convert To EST
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTimeOffset DateTimeConvertToEst(DateTime dt)
        {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return DateTimeConvertTz(tzi, dt);
        }

        /// <summary>
        /// Convert TimeZone
        /// </summary>
        /// <param name="tzi"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTimeOffset DateTimeConvertTz(TimeZoneInfo tzi, DateTime dt)
        {
            DateTimeOffset dtoGMT = new DateTimeOffset(dt, tzi.BaseUtcOffset).UtcDateTime;
            TimeSpan tsbase = TimeZoneInfo.Local.BaseUtcOffset;
            TimeSpan ts = TimeZoneInfo.Local.GetUtcOffset(dtoGMT.LocalDateTime);
            dtoGMT = dt.Add(tzi.BaseUtcOffset); // dtoGMT.Add(tzi.BaseUtcOffset);
            return dt.Add(tzi.BaseUtcOffset);
        }
    }
}
