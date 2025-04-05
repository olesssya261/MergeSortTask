using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MergeSort.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString(parameter?.ToString() ?? "g", culture);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DateTime.TryParse(value?.ToString(), out DateTime result))
            {
                return result;
            }
            return DependencyProperty.UnsetValue; // или вернуть null для nullable DateTime
        }
    }
}
