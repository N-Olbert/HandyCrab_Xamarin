using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace HandyCrab.UI
{
    class IdToBoolConverter : IValueConverter
    {
        public static IValueConverter Converter => new IdToBoolConverter();

        public static string userId;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == userId;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
