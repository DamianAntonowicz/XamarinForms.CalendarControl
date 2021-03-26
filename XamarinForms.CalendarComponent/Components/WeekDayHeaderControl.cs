using System;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public class WeekDayHeaderControl : TemplatedView
    {
        public DayOfWeek DayOfWeek
        {
            get;
        }
        
        public WeekDayHeaderControl(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
        }
    }
}