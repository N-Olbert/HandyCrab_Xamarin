using System;
using System.Globalization;
using Xamarin.Forms;

namespace HandyCrab.UI
{
    public class InverseBoolConverter : IValueConverter
    {
        public static IValueConverter Converter => new InverseBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool?) value ?? true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value as bool?);
        }
    }
}