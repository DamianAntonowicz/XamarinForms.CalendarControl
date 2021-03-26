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
            PickerFirstDayOfWeek.SelectedItem = Calendar.FirstDayOfWeek;
            
            PickerSelectionMode.ItemsSource = Enum.GetValues(typeof(CalendarSelectionMode));
            PickerSelectionMode.SelectedItem = Calendar.SelectionMode;
        }

        private void Calendar_OnDayAdded(object sender, CalendarDayAddedEventArgs e)
        {
            if (e.DayControl.Date.DayOfWeek == DayOfWeek.Saturday ||
                e.DayControl.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                e.DayControl.ControlTemplate = Resources["DayControlTemplateWeekend"] as ControlTemplate;
            }

            if (e.DayControl.Date.Day == 3 ||
                e.DayControl.Date.Day == 10)
            {
                e.DayControl.IsSelectable = false;
            }
            
            if (e.DayControl.Date.Day == 5 ||
                e.DayControl.Date.Day == 16)
            {
                e.DayControl.BindingContext = new Day {HasAppointments = true};
            }

            if (e.DayControl.Date.Month != CalendarControl.Date.Month)
            {
                e.DayControl.IsSelectable = false;
                e.DayControl.ControlTemplate = Resources["DayControlFromOtherMonthTemplate"] as ControlTemplate;
            }
        }

        private void ButtonPreviousMonth_OnClicked(object sender, EventArgs e)
        {
            Calendar.Date = Calendar.Date.AddMonths(-1);
        }
        
        private void ButtonNextMonth_OnClicked(object sender, EventArgs e)
        {
            Calendar.Date = Calendar.Date.AddMonths(1);
        }

        private async void Calendar_OnDayTapped(object sender, CalendarDayTappedEventArgs e)
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

            PickerFirstDayOfWeek.SelectedItem = Calendar.FirstDayOfWeek;
        }
    }

    public class Day
    {
        public bool HasAppointments { get; set; }
    }
}
