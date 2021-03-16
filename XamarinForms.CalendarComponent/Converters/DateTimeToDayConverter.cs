using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Converters
{
    public class DateTimeToDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
        
            var dateTime = (DateTime) value;
            var day = dateTime.Day.ToString();

            return day;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}