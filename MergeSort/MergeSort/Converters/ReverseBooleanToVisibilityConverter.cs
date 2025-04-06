using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MergeSort.Converters
{
    /// <summary>
    /// Обратный конвертер булева значения в Visibility.
    /// True -> Collapsed, False -> Visible.
    /// </summary>
    public class ReverseBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует булево значение в Visibility (инвертированно).
        /// </summary>
        /// <param name="value">Булево значение для конвертации</param>
        /// <param name="targetType">Тип цели (ожидается Visibility)</param>
        /// <param name="parameter">Не используется</param>
        /// <param name="culture">Культура для конвертации</param>
        /// <returns>
        /// Visibility.Collapsed если value равно true, 
        /// Visibility.Visible если false или value не является bool
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, что значение является булевым
            if (value is bool booleanValue)
            {
                // Инвертируем стандартное поведение: true -> Collapsed, false -> Visible
                return booleanValue ? Visibility.Collapsed : Visibility.Visible;
            }
            // Возвращаем Collapsed по умолчанию для небулевых значений
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Преобразует Visibility обратно в булево значение (инвертированно).
        /// </summary>
        /// <param name="value">Значение Visibility для конвертации</param>
        /// <param name="targetType">Тип цели (ожидается bool)</param>
        /// <param name="parameter">Не используется</param>
        /// <param name="culture">Культура для конвертации</param>
        /// <returns>
        /// true если Visibility.Collapsed, false в других случаях
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, что значение является Visibility
            if (value is Visibility visibility)
            {
                // Инвертируем стандартное поведение: Collapsed -> true
                return visibility == Visibility.Collapsed;
            }
            // Возвращаем false по умолчанию
            return false;
        }
    }
}