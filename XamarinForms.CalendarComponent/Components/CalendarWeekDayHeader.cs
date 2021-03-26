using System;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public class CalendarWeekDayHeader : TemplatedView
    {
        public DayOfWeek DayOfWeek
        {
            get;
        }
        
        public CalendarWeekDayHeader(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
        }
    }
}