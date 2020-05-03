using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using HandyCrab.Common.Entitys;


namespace HandyCrab.UI
{
    class VoteToBoolConverter : IValueConverter
    {
        public static IValueConverter Converter => new VoteToBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((string)parameter) {
                case "UpArrow":
                    return (Vote)value == Vote.None || (Vote)value == Vote.Up;
                case "HollowUpArrow":
                    return (Vote)value == Vote.Down;
                case "DownArrow":
                    return (Vote)value == Vote.None || (Vote)value == Vote.Down;
                case "HollowDownArrow":
                    return (Vote)value == Vote.Up;
                default:
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
