using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MergeSort.Converters
{
    /// <summary>
    /// Конвертирует булево значение в значение перечисления Visibility.
    /// True -> Visible, False -> Collapsed.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует булево значение в Visibility.
        /// </summary>
        /// <param name="value">Булево значение для конвертации</param>
        /// <param name="targetType">Тип цели (ожидается Visibility)</param>
        /// <param name="parameter">Не используется</param>
        /// <param name="culture">Культура для конвертации</param>
        /// <returns>
        /// Visibility.Visible если value равно true, 
        /// Visibility.Collapsed если false или value не является bool
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, что значение является булевым
            if (value is bool booleanValue)
            {
                // Возвращаем Visible для true, Collapsed для false
                return booleanValue ? Visibility.Visible : Visibility.Collapsed;
            }
            // Возвращаем Collapsed по умолчанию для небулевых значений
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Преобразует Visibility обратно в булево значение.
        /// </summary>
        /// <param name="value">Значение Visibility для конвертации</param>
        /// <param name="targetType">Тип цели (ожидается bool)</param>
        /// <param name="parameter">Не используется</param>
        /// <param name="culture">Культура для конвертации</param>
        /// <returns>
        /// true если Visibility.Visible, false в других случаях
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, что значение является Visibility
            if (value is Visibility visibility)
            {
                // Возвращаем true только для Visible
                return visibility == Visibility.Visible;
            }
            // Возвращаем false по умолчанию
            return false;
        }
    }
}