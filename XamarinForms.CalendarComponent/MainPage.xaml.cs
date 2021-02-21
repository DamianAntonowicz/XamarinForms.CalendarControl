using System;
using System.Linq;
using Xamarin.Forms;
using XamarinForms.CalendarComponent.Components;

namespace XamarinForms.CalendarComponent
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            PickerFirstDayOfWeek.ItemsSource = Enum.GetValues(typeof(DayOfWeek));
            PickerFirstDayOfWeek.SelectedItem = CalendarControl.FirstDayOfWeek;
            
            PickerSelectionMode.ItemsSource = Enum.GetValues(typeof(CalendarControlSelectionMode));
            PickerSelectionMode.SelectedItem = CalendarControl.SelectionMode;
        }

        private void CalendarControl_OnDayAdded(object sender, DayControlAddedEventArgs e)
        {
            if (e.DayControl.Date.DayOfWeek == DayOfWeek.Saturday ||
                e.DayControl.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                e.DayControl.ControlTemplate = Resources["DayControlTemplate2"] as ControlTemplate;
                e.DayControl.BindingContext = "weekend";
            }

            if (e.DayControl.Date.Day == 1 ||
                e.DayControl.Date.Day == 10)
            {
                e.DayControl.IsSelectable = false;
            }
        }

        private void ButtonPreviousMonth_OnClicked(object sender, EventArgs e)
        {
            CalendarControl.Date = CalendarControl.Date.AddMonths(-1);
        }
        
        private void ButtonNextMonth_OnClicked(object sender, EventArgs e)
        {
            CalendarControl.Date = CalendarControl.Date.AddMonths(1);
        }

        private async void CalendarControl_OnDayTapped(object sender, DayControlTappedEventArgs e)
        {
            //await DisplayAlert(title: "", message: "You clicked on: " + e.DayControl.Date, cancel: "ok");
        }

        private void CheckBoxShowWeekends_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (CheckBoxShowWeekends.IsChecked)
            {
                PickerFirstDayOfWeek.ItemsSource = Enum.GetValues(typeof(DayOfWeek));
            }
            else
            {
                var values = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
                values.Remove(DayOfWeek.Saturday);
                values.Remove(DayOfWeek.Sunday);

                PickerFirstDayOfWeek.ItemsSource = values;
            }

            PickerFirstDayOfWeek.SelectedItem = CalendarControl.FirstDayOfWeek;
        }
    }
}
