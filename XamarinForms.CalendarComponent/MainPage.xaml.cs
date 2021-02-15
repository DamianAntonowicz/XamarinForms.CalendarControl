using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
