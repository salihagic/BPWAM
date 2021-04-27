using System;

namespace BPWA.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FromUtc(this DateTime dateTime, TimeZoneInfo timezone)
        {
            if (dateTime == null || timezone == null)
                return dateTime;

            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, timezone);
        }

        public static DateTime ToUtc(this DateTime dateTime, TimeZoneInfo timezone)
        {
            if (dateTime == null || timezone == null)
                return dateTime;

            return TimeZoneInfo.ConvertTime(dateTime, timezone, TimeZoneInfo.Utc);
        }
    }
}
