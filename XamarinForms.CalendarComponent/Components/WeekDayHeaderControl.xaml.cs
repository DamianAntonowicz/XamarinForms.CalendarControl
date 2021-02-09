using System;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public partial class WeekDayHeaderControl : ContentView
    {
        #region DayOfWeekProperty

        public static readonly BindableProperty DayOfWeekProperty =
            BindableProperty.Create(
                propertyName: nameof(DayOfWeek),
                returnType: typeof(DayOfWeek),
                declaringType: typeof(WeekDayHeaderControl));

        public DayOfWeek DayOfWeek
        {
            get => (DayOfWeek) GetValue(DayOfWeekProperty);
            set => SetValue(DayOfWeekProperty, value);
        }
        
        #endregion
        
        public WeekDayHeaderControl()
        {
            InitializeComponent();
        }
    }
}