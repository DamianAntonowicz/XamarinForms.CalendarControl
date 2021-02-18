using System.Globalization;

namespace System
{
    public static class DateTimeExtensions
    {
        public static int DaysInMonth(this DateTime dateTime)
        {
            return DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        }

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static int WeeksInMonth(this DateTime dateTime)
        {
            var daysInMonth = DaysInMonth(dateTime);
            var lastWeekOfMonth= WeekOfMonth(new DateTime(dateTime.Year, dateTime.Month, daysInMonth));

            return lastWeekOfMonth;
        }

        public static int WeekOfMonth(this DateTime date)
        {
            var weekOfYear = date.WeekOfYear();
            var weekOfYearForFirstDayOfMonth = date.FirstDayOfMonth().WeekOfYear();
            var weekOfMonth = weekOfYear - weekOfYearForFirstDayOfMonth + 1;

            return weekOfMonth;
        }

        public static int DayOfWeek(this DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case System.DayOfWeek.Monday: return 1;
                case System.DayOfWeek.Tuesday: return 2;
                case System.DayOfWeek.Wednesday: return 3;
                case System.DayOfWeek.Thursday: return 4;
                case System.DayOfWeek.Friday: return 5;
                case System.DayOfWeek.Saturday: return 6;
                case System.DayOfWeek.Sunday: return 7;
            }

            return default;
        }
        
        private static int WeekOfYear(this DateTime dateTime)
        {
            var weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime,
                rule: CalendarWeekRule.FirstDay,
                firstDayOfWeek: System.DayOfWeek.Monday);

            return weekOfYear;
        }
    }
}