using System.Globalization;

namespace System
{
    public static class DateTimeExtensions
    {
        public static int DaysInMonth(this DateTime dateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

            return daysInMonth;
        }

        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);

            return firstDayOfMonth;
        }

        public static int WeeksInMonth(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            var daysInMonth = DaysInMonth(dateTime);
            var date = new DateTime(dateTime.Year, dateTime.Month, daysInMonth);
            var lastWeekOfMonth= WeekOfMonth(date, firstDayOfWeek);

            return lastWeekOfMonth;
        }

        public static int WeekOfMonth(this DateTime date, DayOfWeek firstDayOfWeek)
        {
            var weekOfYear = date.WeekOfYear(firstDayOfWeek);
            var weekOfYearForFirstDayOfMonth = date.FirstDayOfMonth().WeekOfYear(firstDayOfWeek);
            var weekOfMonth = weekOfYear - weekOfYearForFirstDayOfMonth + 1;

            return weekOfMonth;
        }

        public static int DayOfWeek(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            var currentDayOfWeek = firstDayOfWeek;
            var dayOfWeek = 1;
            
            for (var i = 1; i <= 7; i++)
            {
                if (currentDayOfWeek == dateTime.DayOfWeek)
                {
                    break;
                }
                
                currentDayOfWeek = currentDayOfWeek.NextOrFirst();
                dayOfWeek++;
            }

            return dayOfWeek;
        }
        
        private static int WeekOfYear(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            var weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime,
                rule: CalendarWeekRule.FirstDay,
                firstDayOfWeek: firstDayOfWeek);

            return weekOfYear;
        }
    }
}