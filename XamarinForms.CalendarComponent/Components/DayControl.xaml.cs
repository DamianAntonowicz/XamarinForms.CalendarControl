using System;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public partial class DayControl : ContentView
    {
        public const int DaysInWeek = 7;

        public event EventHandler Tapped;

        #region IsSelectedProperty

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                nameof(IsSelected),
                typeof(bool),
                typeof(bool));

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        #endregion

        #region DateProperty

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(
                nameof(Date),
                typeof(DateTime),
                typeof(DateTime));

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