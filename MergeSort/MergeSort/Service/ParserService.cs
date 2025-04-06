using System.Globalization;

namespace MergeSort.Service
{
    public static class ParserService
    {
        // Культура используется для гарантии, что точка будет использоваться как десятичный разделитель
        private static readonly CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Преобразует строку в массив чисел double, разделённых пробелами.
        /// </summary>
        /// <param name="input">Входная строка, содержащая числа, разделённые пробелами</param>
        /// <returns>Массив чисел типа double</returns>
        /// <exception cref="FormatException">Выбрасывается, если строка содержит некорректное число</exception>
        public static double[] ParseStringToDoubleArray(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            // Проверка на наличие запятых
            if (input.Contains(','))
            {
                throw new FormatException("Использование запятых в качестве разделителей запрещено. Используйте пробелы.");
            }

            // Разделение строки по пробелам
            string[] stringValues = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            double[] result = new double[stringValues.Length];

            // Преобразуем каждую строку в число double
            for (int i = 0; i < stringValues.Length; i++)
            {
                if (!double.TryParse(stringValues[i], NumberStyles.Any, culture, out result[i]))
                {
                    throw new FormatException($"Неверный формат числа: '{stringValues[i]}'");
                }
            }

            return result;
        }

        /// <summary>
        /// Преобразует массив чисел double в строку, разделённую пробелами.
        /// </summary>
        /// <param name="array">Массив чисел типа double</param>
        /// <returns>Строка, содержащая числа, разделённые пробелами</returns>
        public static string ParseDoubleArrayToString(double[] array)
        {
            if (array == null)
                return null;

            // Преобразуем каждый элемент массива в строку и соединяем их пробелами
            return string.Join(" ", Array.ConvertAll(array, x => x.ToString(culture)));
        }
    }
}
