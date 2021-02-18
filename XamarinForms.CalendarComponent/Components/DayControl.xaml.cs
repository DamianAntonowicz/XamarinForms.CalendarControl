using System;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public partial class DayControl : ContentView
    {
        public event EventHandler Tapped;

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

        #region DateProperty

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(
                propertyName: nameof(Date),
                returnType: typeof(DateTime),
                declaringType: typeof(DayControl));

        public DateTime Date
        {
            get => (DateTime) GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        #endregion

        public DayControl()
        {
            InitializeComponent();
        }

        private void DayControl_OnTapped(object sender, EventArgs e)
        {
            Tapped?.Invoke(this, null);
        }
    }
}