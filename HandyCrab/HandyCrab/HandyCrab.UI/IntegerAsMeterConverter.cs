using System;
using System.Globalization;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HandyCrab.UI
{
    class IntegerAsMeterConverter : IValueConverter
    {
        public static IValueConverter Converter => new IntegerAsMeterConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<String> returnValue = new ObservableCollection<String>();
            for (int i = 0; i < ((int[])value).Length; i++)
            {
                returnValue.Add(((int[])value)[i].ToString() + "m");
            }
            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
