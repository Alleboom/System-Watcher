using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System_Watcher_MVVM.Helpers.Converters.DateTimeConverter
{
    public static class DateTimeTimeZoneConversion
    {

        
        /// <summary>
        /// Converts a UTC time to a local time judged by the system
        /// </summary>
        /// <param name="_UTCDateTime">The UTC date/time to modify</param>
        /// <returns>A localized Date and Time</returns>
        public static DateTime ConvertTimeZone(DateTime _UTCDateTime)
        {
            var _timeZone = TimeZoneInfo.Local;

            return TimeZoneInfo.ConvertTimeFromUtc(_UTCDateTime, _timeZone);
        }

        /// <summary>
        /// Converts a UTC time to a local time judged by the system
        /// </summary>
        /// <param name="_UTCDateTime">The UTC date/time to modify, can be nullable</param>
        /// <returns>A localized Date and Time</returns>
        public static DateTime? ConvertTimeZone(DateTime? _UTCDateTime)
        {
            if (_UTCDateTime.HasValue)
            {
                return ConvertTimeZone(_UTCDateTime.Value);
            }
            else
            {
                return null;
            }
        }
    }
}
