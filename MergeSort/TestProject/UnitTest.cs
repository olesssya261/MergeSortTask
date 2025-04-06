using MergeSort.DbLocator;
using MergeSort.Model.ObservableModels;
using MergeSort.Service;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SortedModel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace MergeSort.Tests
{
    [TestFixture]
    [Timeout(300000)] 
    public class DatabaseTests
    {
        private Random _random;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var context = DbContextSingleton.Instance;
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DbContextSingleton.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _random = new Random();
            var context = DbContextSingleton.Instance;
            context.Arrays.RemoveRange(context.Arrays);
            context.SaveChanges();
        }

        private double[] GenerateRandomArray(int size)
        {
            var array = new double[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = _random.NextDouble() * 1000;
            }
            return array;
        }

        // Вспомогательный метод для проверки, что массив отсортирован по возрастанию
        private bool IsSorted(double[] array)
        {
            if (array == null || array.Length <= 1) return true;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < array[i - 1])
                    return false;
            }
            return true;
        }

        [Test]
        public void TestAdd100Arrays()
        {
            const int arrayCount = 100;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                for (int i = 0; i < arrayCount; i++)
                {
                    int arraySize = _random.Next(5, 50);
                    var array = GenerateRandomArray(arraySize);

                    var model = new SortArrayObservableModel
                    {
                        ArrayData = ParserService.ParseDoubleArrayToString(array),
                        CreatedAt = DateTime.Now
                    };
                    model.Save();
                }

                stopwatch.Stop();
                TestContext.WriteLine($"Успешно добавлено {arrayCount} массивов. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Assert.Fail($"Ошибка при добавлении {arrayCount} массивов: {ex.Message}. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
        }

        [Test]
        public void TestAdd1000Arrays()
        {
            const int arrayCount = 1000;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                for (int i = 0; i < arrayCount; i++)
                {
                    int arraySize = _random.Next(5, 50);
                    var array = GenerateRandomArray(arraySize);

                    var model = new SortArrayObservableModel
                    {
                        ArrayData = ParserService.ParseDoubleArrayToString(array),
                        CreatedAt = DateTime.Now
                    };
                    model.Save();
                }

                stopwatch.Stop();
                TestContext.WriteLine($"Успешно добавлено {arrayCount} массивов. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Assert.Fail($"Ошибка при добавлении {arrayCount} массивов: {ex.Message}. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
        }

        [Test]
        public void TestAdd10000Arrays()
        {
            const int arrayCount = 10000;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                for (int i = 0; i < arrayCount; i++)
                {
                    int arraySize = _random.Next(5, 50);
                    var array = GenerateRandomArray(arraySize);

                    var model = new SortArrayObservableModel
                    {
                        ArrayData = ParserService.ParseDoubleArrayToString(array),
                        CreatedAt = DateTime.Now
                    };
                    model.Save();
                }

                stopwatch.Stop();
                TestContext.WriteLine($"Успешно добавлено {arrayCount} массивов. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Assert.Fail($"Ошибка при добавлении {arrayCount} массивов: {ex.Message}. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
        }

        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void TestLoadAndSortAllArrays(int totalArrays)
        {
            const int batchSize = 1000;
            int remainingArrays = totalArrays;
            while (remainingArrays > 0)
            {
                int currentBatchSize = Math.Min(batchSize, remainingArrays);
                for (int i = 0; i < currentBatchSize; i++)
                {
                    int arraySize = _random.Next(5, 50);
                    var array = GenerateRandomArray(arraySize);
                    var model = new SortArrayObservableModel
                    {
                        ArrayData = ParserService.ParseDoubleArrayToString(array),
                        CreatedAt = DateTime.Now
                    };
                    model.Save();
                }

                remainingArrays -= currentBatchSize;
                TestContext.WriteLine($"Добавлено {currentBatchSize} массивов. Осталось: {remainingArrays}");
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                const int arraysToSort = 100; // Сортируем только 100 случайных массивов
                var context = DbContextSingleton.Instance;
                var allArrays = context.Arrays.ToList();
                var selectedArrays = allArrays.OrderBy(x => _random.Next()).Take(arraysToSort).ToList();

                foreach (var arrayModel in selectedArrays)
                {
                    var observableModel = new SortArrayObservableModel(arrayModel);
                    var array = ParserService.ParseStringToDoubleArray(observableModel.ArrayData);

                    // Сортируем с помощью MergeSort
                    var (swaps, comparisons, sortedArray) = SortModel.MergeSort(array);

                    // Проверяем, что массив отсортирован корректно
                    Assert.IsTrue(IsSorted(sortedArray), $"Массив с ID {arrayModel.Id} не был отсортирован корректно с помощью MergeSort");

                    // Обновляем объект в памяти (но не сохраняем в базу данных)
                    observableModel.SortedArrayData = ParserService.ParseDoubleArrayToString(sortedArray);
                    observableModel.SortType = "MergeSort";
                    observableModel.Swaps = swaps;
                    observableModel.Comparisons = comparisons;
                }

                stopwatch.Stop();
                double averageTimePerArray = stopwatch.ElapsedMilliseconds / (double)arraysToSort;
                TestContext.WriteLine($"Успешно отсортировано {arraysToSort} массивов из {totalArrays} с использованием MergeSort. Общее время: {stopwatch.ElapsedMilliseconds} мс. Среднее время на массив: {averageTimePerArray:F2} мс");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Assert.Fail($"Ошибка при сортировке 100 массивов из {totalArrays} с использованием MergeSort: {ex.Message}. Общее время: {stopwatch.ElapsedMilliseconds} мс");
            }
        }

        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void TestClearDatabase(int totalArrays)
        {
            for (int i = 0; i < totalArrays; i++)
            {
                int arraySize = _random.Next(5, 50);
                var array = GenerateRandomArray(arraySize);

                var model = new SortArrayObservableModel
                {
                    ArrayData = ParserService.ParseDoubleArrayToString(array),
                    CreatedAt = DateTime.Now
                };
                model.Save();
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var context = DbContextSingleton.Instance;
                context.Arrays.RemoveRange(context.Arrays);
                context.SaveChanges();

                stopwatch.Stop();
                TestContext.WriteLine($"Успешно очищена база данных с {totalArrays} записями. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Assert.Fail($"Ошибка при очистке базы данных с {totalArrays} записями: {ex.Message}. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
        }

        [Test]
        public void TestBubbleSortCorrectness()
        {
            var array = new double[] { 5, 2, 8, 1, 9 };
            var expected = new double[] { 1, 2, 5, 8, 9 };

            var (swaps, comparisons, sortedArray) = SortModel.BubbleSort(array);

            Assert.IsTrue(sortedArray.SequenceEqual(expected), "BubbleSort не отсортировал массив корректно");
            Assert.Greater(swaps, 0, "BubbleSort должен был выполнить перестановки");
            Assert.Greater(comparisons, 0, "BubbleSort должен был выполнить сравнения");
        }

        [Test]
        public void TestMergeSortCorrectness()
        {
            var array = new double[] { 5, 2, 8, 1, 9 };
            var expected = new double[] { 1, 2, 5, 8, 9 };

            var (swaps, comparisons, sortedArray) = SortModel.MergeSort(array);

            Assert.IsTrue(sortedArray.SequenceEqual(expected), "MergeSort не отсортировал массив корректно");
            Assert.Greater(swaps, 0, "MergeSort должен был выполнить перестановки");
            Assert.Greater(comparisons, 0, "MergeSort должен был выполнить сравнения");
        }

        [Test]
        public void TestQuickSortCorrectness()
        {
            var array = new double[] { 5, 2, 8, 1, 9 };
            var expected = new double[] { 1, 2, 5, 8, 9 };

            var (swaps, comparisons, sortedArray) = SortModel.QuickSort(array);

            Assert.IsTrue(sortedArray.SequenceEqual(expected), "QuickSort не отсортировал массив корректно");
            Assert.Greater(swaps, 0, "QuickSort должен был выполнить перестановки");
            Assert.Greater(comparisons, 0, "QuickSort должен был выполнить сравнения");
        }

        [Test]
        public void TestBubbleSortNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => SortModel.BubbleSort(null));
        }

        [Test]
        public void TestMergeSortNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => SortModel.MergeSort(null));
        }

        [Test]
        public void TestQuickSortNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => SortModel.QuickSort(null));
        }
    }
}