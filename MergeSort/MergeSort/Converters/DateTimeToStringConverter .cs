using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MergeSort.Converters
{
    /// <summary>
    /// Конвертирует DateTime в строковое представление и обратно.
    /// </summary>
    public class DateTimeToStringConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует DateTime в строку с использованием формата.
        /// </summary>
        /// <param name="value">DateTime для конвертации</param>
        /// <param name="targetType">Тип цели (ожидается string)</param>
        /// <param name="parameter">Формат строки (по умолчанию "g")</param>
        /// <param name="culture">Культура для форматирования</param>
        /// <returns>
        /// Строковое представление даты или пустая строка, если value не DateTime
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, что значение является DateTime
            if (value is DateTime dateTime)
            {
                // Используем переданный формат или "g" по умолчанию
                return dateTime.ToString(parameter?.ToString() ?? "g", culture);
            }
            // Возвращаем пустую строку для не-DateTime значений
            return string.Empty;
        }

        /// <summary>
        /// Преобразует строку обратно в DateTime.
        /// </summary>
        /// <param name="value">Строка для парсинга</param>
        /// <param name="targetType">Тип цели (ожидается DateTime)</param>
        /// <param name="parameter">Не используется</param>
        /// <param name="culture">Культура для парсинга</param>
        /// <returns>
        /// Распарсенное значение DateTime или DependencyProperty.UnsetValue при ошибке
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Пытаемся распарсить строку в DateTime
            if (DateTime.TryParse(value?.ToString(), out DateTime result))
            {
                return result;
            }
            // Возвращаем специальное значение при ошибке парсинга
            return DependencyProperty.UnsetValue;
        }
    }
}