using System;
using System.Globalization;
using Xamarin.Forms;

namespace HandyCrab.UI
{
    class NullValueBoolConverter : IValueConverter
    {
        public static IValueConverter Converter => new NullValueBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
