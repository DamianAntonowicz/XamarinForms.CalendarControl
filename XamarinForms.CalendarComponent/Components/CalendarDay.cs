using System;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public class CalendarDay : TemplatedView
    {
        #region IsSelectableProperty

        public static readonly BindableProperty IsSelectableProperty =
            BindableProperty.Create(
                propertyName: nameof(IsSelectable),
                returnType: typeof(bool),
                defaultValue: true,
                declaringType: typeof(CalendarDay));

        public bool IsSelectable
        {
            get => (bool) GetValue(IsSelectableProperty);
            set => SetValue(IsSelectableProperty, value);
        }

        #endregion
        
        #region IsSelectedProperty

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                propertyName: nameof(IsSelected),
                returnType: typeof(bool),
                declaringType: typeof(CalendarDay));

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        #endregion

        #region DateProperty

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(
                propertyName: nameof(Date),
                returnType: typeof(DateTime),
                declaringType: typeof(CalendarDay));

        public DateTime Date
        {
            get => (DateTime) GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        #endregion
    }
}