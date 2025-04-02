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
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            uint swapCount = 0;
            uint comparisonCount = 0;
            double[] sortedArray = (double[])array.Clone();

            for (int i = 0; i < sortedArray.Length - 1; i++)
            {
                for (int j = 0; j < sortedArray.Length - i - 1; j++)
                {
                    comparisonCount++;
                    if (sortedArray[j] > sortedArray[j + 1])
                    {
                        // Перестановка элементов
                        (sortedArray[j], sortedArray[j + 1]) = (sortedArray[j + 1], sortedArray[j]);
                        swapCount++;
                    }
                }
            }

            return (swapCount, comparisonCount, sortedArray);
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
            double[] sortedArray = (double[])array.Clone();

            MergeSortInternal(sortedArray, 0, sortedArray.Length - 1, ref swapCount, ref comparisonCount);

            return (swapCount, comparisonCount, sortedArray);
        }

        private static void MergeSortInternal(double[] array, int left, int right, ref uint swaps, ref uint comparisons)
        {
            if (left < right)
            {
                int middle = left + (right - left) / 2;

                MergeSortInternal(array, left, middle, ref swaps, ref comparisons);
                MergeSortInternal(array, middle + 1, right, ref swaps, ref comparisons);

                Merge(array, left, middle, right, ref swaps, ref comparisons);
            }
        }

        private static void Merge(double[] array, int left, int middle, int right, ref uint swaps, ref uint comparisons)
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;

            double[] leftArray = new double[n1];
            double[] rightArray = new double[n2];

            Array.Copy(array, left, leftArray, 0, n1);
            Array.Copy(array, middle + 1, rightArray, 0, n2);

            int i = 0, j = 0, k = left;

            while (i < n1 && j < n2)
            {
                comparisons++;
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
                swaps++;
                k++;
            }

            while (i < n1)
            {
                array[k] = leftArray[i];
                i++;
                k++;
                swaps++;
            }

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
            double[] sortedArray = (double[])array.Clone();

            QuickSortInternal(sortedArray, 0, sortedArray.Length - 1, ref swapCount, ref comparisonCount);

            return (swapCount, comparisonCount, sortedArray);
        }

        private static void QuickSortInternal(double[] array, int low, int high, ref uint swaps, ref uint comparisons)
        {
            if (low < high)
            {
                int partitionIndex = Partition(array, low, high, ref swaps, ref comparisons);

                QuickSortInternal(array, low, partitionIndex - 1, ref swaps, ref comparisons);
                QuickSortInternal(array, partitionIndex + 1, high, ref swaps, ref comparisons);
            }
        }

        private static int Partition(double[] array, int low, int high, ref uint swaps, ref uint comparisons)
        {
            double pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                comparisons++;
                if (array[j] < pivot)
                {
                    i++;
                    (array[i], array[j]) = (array[j], array[i]);
                    swaps++;
                }
            }

            (array[i + 1], array[high]) = (array[high], array[i + 1]);
            swaps++;

            return i + 1;
        }
    }
}
