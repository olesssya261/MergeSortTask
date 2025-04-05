using System.Globalization;

namespace MergeSort.Service
{
    public static class ParserService
    {
        private static readonly CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Преобразует строку в массив double, используя пробелы как разделители
        /// и точку как десятичный разделитель.
        /// </summary>
        /// <param name="input">Входная строка с числами</param>
        /// <returns>Массив чисел double</returns>
        /// <exception cref="ArgumentNullException">Если входная строка null</exception>
        /// <exception cref="FormatException">Если строка содержит недопустимые символы</exception>
        public static double[] ParseStringToDoubleArray(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string[] stringValues = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            double[] result = new double[stringValues.Length];

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
        /// Преобразует массив double в строку, используя пробелы как разделители
        /// и точку как десятичный разделитель.
        /// </summary>
        /// <param name="array">Массив чисел для преобразования</param>
        /// <returns>Строка с числами, разделенными пробелами</returns>
        /// <exception cref="ArgumentNullException">Если массив равен null</exception>
        public static string ParseDoubleArrayToString(double[] array)
        {
            if (array == null)
                return null;

            return string.Join(" ", Array.ConvertAll(array, x => x.ToString(culture)));
        }
    }
}
