using System;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public class DayControl : TemplatedView
    {
        #region IsSelectableProperty

        public static readonly BindableProperty IsSelectableProperty =
            BindableProperty.Create(
                propertyName: nameof(IsSelectable),
                returnType: typeof(bool),
                defaultValue: true,
                declaringType: typeof(DayControl));

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
                declaringType: typeof(DayControl));

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        #endregion
        
        public DateTime Date
        {
            get;
        }

        public DayControl(DateTime date)
        {
            Date = date;
        }
    }
}