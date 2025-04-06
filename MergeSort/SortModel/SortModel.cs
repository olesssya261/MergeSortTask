namespace SortedModel
{
    public static class SortModel
    {
        /// <summary>
        /// Сортировка пузырьком
        /// </summary>
        /// <param name="array">Исходный массив</param>
        /// <returns>(перестановки, сравнения, отсортированный массив)</returns>
        public static (uint swaps, uint comparisons, double[] sortedArray) BubbleSort(double[] array)
        {
            // Проверка на null для массива
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            uint swapCount = 0; // Счётчик перестановок
            uint comparisonCount = 0; // Счётчик сравнений
            double[] sortedArray = (double[])array.Clone(); // Создаем клон массива для сортировки

            // Два вложенных цикла для сравнения и перестановки элементов массива
            for (int i = 0; i < sortedArray.Length - 1; i++)
            {
                for (int j = 0; j < sortedArray.Length - i - 1; j++)
                {
                    comparisonCount++; // Увеличиваем счётчик сравнений
                    if (sortedArray[j] > sortedArray[j + 1])
                    {
                        // Перестановка элементов
                        (sortedArray[j], sortedArray[j + 1]) = (sortedArray[j + 1], sortedArray[j]);
                        swapCount++; // Увеличиваем счётчик перестановок
                    }
                }
            }

            return (swapCount, comparisonCount, sortedArray); // Возвращаем результат сортировки
        }

        /// <summary>
        /// Сортировка слиянием
        /// </summary>
        /// <param name="array">Исходный массив</param>
        /// <returns>(перестановки, сравнения, отсортированный массив)</returns>
        public static (uint swaps, uint comparisons, double[] sortedArray) MergeSort(double[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            uint swapCount = 0;
            uint comparisonCount = 0;
            double[] sortedArray = (double[])array.Clone(); // Создаем клон массива для сортировки

            // Рекурсивная сортировка массива с помощью слияния
            MergeSortInternal(sortedArray, 0, sortedArray.Length - 1, ref swapCount, ref comparisonCount);

            return (swapCount, comparisonCount, sortedArray); // Возвращаем результат сортировки
        }

        private static void MergeSortInternal(double[] array, int left, int right, ref uint swaps, ref uint comparisons)
        {
            // Базовое условие рекурсии
            if (left < right)
            {
                int middle = left + (right - left) / 2;

                MergeSortInternal(array, left, middle, ref swaps, ref comparisons); // Рекурсивная сортировка левой части
                MergeSortInternal(array, middle + 1, right, ref swaps, ref comparisons); // Рекурсивная сортировка правой части

                // Слияние отсортированных частей
                Merge(array, left, middle, right, ref swaps, ref comparisons);
            }
        }

        private static void Merge(double[] array, int left, int middle, int right, ref uint swaps, ref uint comparisons)
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;

            // Создаём временные массивы для хранения левой и правой части
            double[] leftArray = new double[n1];
            double[] rightArray = new double[n2];

            // Копируем данные в временные массивы
            Array.Copy(array, left, leftArray, 0, n1);
            Array.Copy(array, middle + 1, rightArray, 0, n2);

            int i = 0, j = 0, k = left;

            // Объединяем временные массивы в один отсортированный массив
            while (i < n1 && j < n2)
            {
                comparisons++; // Считаем количество сравнений
                if (leftArray[i] <= rightArray[j])
                {
                    array[k] = leftArray[i];
                    i++;
                }
                else
                {
                    array[k] = rightArray[j];
                    j++;
                }
                swaps++; // Считаем количество перестановок
                k++;
            }

            // Если остались элементы в левой части, копируем их в основной массив
            while (i < n1)
            {
                array[k] = leftArray[i];
                i++;
                k++;
                swaps++;
            }

            // Если остались элементы в правой части, копируем их в основной массив
            while (j < n2)
            {
                array[k] = rightArray[j];
                j++;
                k++;
                swaps++;
            }
        }

        /// <summary>
        /// Быстрая сортировка (QuickSort)
        /// </summary>
        /// <param name="array">Исходный массив</param>
        /// <returns>(перестановки, сравнения, отсортированный массив)</returns>
        public static (uint swaps, uint comparisons, double[] sortedArray) QuickSort(double[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            uint swapCount = 0;
            uint comparisonCount = 0;
            double[] sortedArray = (double[])array.Clone(); // Создаем клон массива для сортировки

            // Рекурсивная быстрая сортировка
            QuickSortInternal(sortedArray, 0, sortedArray.Length - 1, ref swapCount, ref comparisonCount);

            return (swapCount, comparisonCount, sortedArray); // Возвращаем результат сортировки
        }

        private static void QuickSortInternal(double[] array, int low, int high, ref uint swaps, ref uint comparisons)
        {
            if (low < high)
            {
                // Разделяем массив на две части
                int partitionIndex = Partition(array, low, high, ref swaps, ref comparisons);

                // Рекурсивно сортируем обе части массива
                QuickSortInternal(array, low, partitionIndex - 1, ref swaps, ref comparisons);
                QuickSortInternal(array, partitionIndex + 1, high, ref swaps, ref comparisons);
            }
        }

        private static int Partition(double[] array, int low, int high, ref uint swaps, ref uint comparisons)
        {
            double pivot = array[high]; // Опорный элемент
            int i = low - 1;

            // Перебираем элементы массива и переставляем их относительно опорного элемента
            for (int j = low; j < high; j++)
            {
                comparisons++; // Считаем количество сравнений
                if (array[j] < pivot)
                {
                    i++;
                    (array[i], array[j]) = (array[j], array[i]); // Переставляем элементы
                    swaps++; // Считаем количество перестановок
                }
            }

            // Переставляем опорный элемент на его правильное место
            (array[i + 1], array[high]) = (array[high], array[i + 1]);
            swaps++; // Считаем количество перестановок

            return i + 1; // Возвращаем индекс опорного элемента
        }
    }
}
