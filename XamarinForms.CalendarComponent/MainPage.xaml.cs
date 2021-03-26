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
            if (e.CalendarDay.Date.DayOfWeek == DayOfWeek.Saturday ||
                e.CalendarDay.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                e.CalendarDay.ControlTemplate = Resources["CalendarDayControlTemplateWeekend"] as ControlTemplate;
            }

            if (e.CalendarDay.Date.Day == 3 ||
                e.CalendarDay.Date.Day == 10)
            {
                e.CalendarDay.IsSelectable = false;
            }
            
            if (e.CalendarDay.Date.Day == 5 ||
                e.CalendarDay.Date.Day == 16)
            {
                e.CalendarDay.BindingContext = new Day {HasAppointments = true};
            }

            if (e.CalendarDay.Date.Month != Calendar.Date.Month)
            {
                e.CalendarDay.IsSelectable = false;
                e.CalendarDay.ControlTemplate = Resources["CalendarDayFromOtherMonthControlTemplate"] as ControlTemplate;
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
            // await DisplayAlert(title: "", message: "You clicked on: " + e.CalendarDay.Date, cancel: "ok");
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
