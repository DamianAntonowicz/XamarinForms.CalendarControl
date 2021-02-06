using System.Globalization;

namespace System
{
    public static class DateTimeExtensions
    {

        public static int GetDayCountInMonth(this DateTime dateTime)
        {
            return DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        }

        public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static int GetWeekCountInMonth(this DateTime dateTime)
        {
            return GetWeekNumberOfMonth(new DateTime(dateTime.Year, dateTime.Month, GetDayCountInMonth(dateTime)));
        }

        public static int GetWeekNumberOfMonth(this DateTime date)
        {
            return date.GetWeekNumberOfYear() - date.GetFirstDayOfMonth().GetWeekNumberOfYear() + 1;
        }

        public static int GetDayNumberOfWeek(this DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 7;
            }

            return default;
        }
        
        private static int GetWeekNumberOfYear(this DateTime dateTime)
        {
            var culture = CultureInfo.CurrentCulture;

            return culture.Calendar.GetWeekOfYear(dateTime,
                CalendarWeekRule.FirstDay,
                DayOfWeek.Monday);
        }
    }
}