using System;
using Xamarin.Forms;
using XamarinForms.CalendarComponent.Components;

namespace XamarinForms.CalendarComponent
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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
        
        private void ButtonSingleSelect_OnClicked(object sender, EventArgs e)
        {
            CalendarControl.SelectionMode = CalendarControlSelectionMode.SingleSelect;
        }
        
        private void ButtonMultiselect_OnClicked(object sender, EventArgs e)
        {
            CalendarControl.SelectionMode = CalendarControlSelectionMode.MultiSelect;
        }

        private async void CalendarControl_OnDayTapped(object sender, DayControlTappedEventArgs e)
        {
            //await DisplayAlert(title: "", message: "You clicked on: " + e.DayControl.Date, cancel: "ok");
        }
    }
}
