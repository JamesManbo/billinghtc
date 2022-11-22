using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Utility
{
    public static class DateTimeHelperExtension
    {
        public static int LocalTimeZone = 7;
        public static bool LessThanDate(this DateTime source, DateTime date)
        {
            return source.AddHours(LocalTimeZone).Date < date.AddHours(LocalTimeZone).Date;
        }

        public static bool GreaterThanDate(this DateTime source, DateTime date)
        {
            return source.AddHours(LocalTimeZone).Date > date.AddHours(LocalTimeZone).Date;
        }

        public static bool LessThanOrEqualDate(this DateTime source, DateTime date)
        {
            return source.AddHours(LocalTimeZone).Date <= date.AddHours(LocalTimeZone).Date;
        }

        public static bool GreaterThanOrEqualDate(this DateTime source, DateTime date)
        {
            return source.AddHours(LocalTimeZone).Date >= date.AddHours(LocalTimeZone).Date;
        }

        public static bool EqualityDate(this DateTime source, DateTime date)
        {
            return source.AddHours(LocalTimeZone).Date == date.AddHours(LocalTimeZone).Date;
        }

        public static bool IsLastDayInMonth(this DateTime source)
        {
            var localDate = source.AddHours(LocalTimeZone);
            return localDate.Day == DateTime.DaysInMonth(localDate.Year, localDate.Month);
        }

        public static DateTime ToExactLocalDate(this DateTime source)
        {
            var localDate = source.AddHours(LocalTimeZone);
            var result = new DateTime(localDate.Year, localDate.Month, localDate.Day, 0, 0, 0);
            return result;
        }
    }
}
