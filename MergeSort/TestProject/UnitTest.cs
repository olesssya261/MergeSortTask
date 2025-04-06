using MergeSort.DbLocator;
using MergeSort.Model.ObservableModels;
using MergeSort.Service;
using SortedModel;
using System.Diagnostics;

namespace MergeSort.Tests
{
    [TestFixture]
    [Timeout(300000)] // Устанавливаем таймаут для всех тестов в 5 минут
    public class DatabaseTests
    {
        private Random _random;

        /// <summary>
        /// Метод, который выполняется один раз перед всеми тестами.
        /// Создает новую базу данных, очищая старую.
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var context = DbContextSingleton.Instance;
            context.Database.EnsureDeleted(); // Удаляем существующую базу данных
            context.Database.EnsureCreated(); // Создаем новую базу данных
        }

        /// <summary>
        /// Метод, который выполняется один раз после всех тестов.
        /// Освобождает ресурсы, связанные с базой данных.
        /// </summary>
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DbContextSingleton.Dispose(); // Освобождаем ресурсы контекста базы данных
        }

        /// <summary>
        /// Метод, который выполняется перед каждым тестом.
        /// Очищает старые данные в базе данных перед тестами.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _random = new Random(); // Инициализация генератора случайных чисел
            var context = DbContextSingleton.Instance;
            context.Arrays.RemoveRange(context.Arrays); // Удаляем все данные массивов
            context.SaveChanges(); // Сохраняем изменения
        }

        /// <summary>
        /// Генерирует случайный массив вещественных чисел заданного размера.
        /// </summary>
        /// <param name="size">Размер массива</param>
        /// <returns>Сгенерированный массив чисел</returns>
        private double[] GenerateRandomArray(int size)
        {
            var array = new double[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = _random.NextDouble() * 1000; // Заполняем массив случайными числами
            }
            return array;
        }

        /// <summary>
        /// Проверяет, что массив отсортирован по возрастанию.
        /// </summary>
        /// <param name="array">Массив для проверки</param>
        /// <returns>True, если массив отсортирован, иначе false</returns>
        private bool IsSorted(double[] array)
        {
            if (array == null || array.Length <= 1) return true; // Массив считается отсортированным, если его длина <= 1
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < array[i - 1]) // Если текущий элемент меньше предыдущего, массив не отсортирован
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Тестирует добавление 100 массивов в базу данных.
        /// </summary>
        [Test]
        public void TestAdd100Arrays()
        {
            const int arrayCount = 100; // Число массивов для добавления
            var stopwatch = Stopwatch.StartNew(); // Начинаем отсчет времени

            try
            {
                // Добавляем 100 случайных массивов в базу данных
                for (int i = 0; i < arrayCount; i++)
                {
                    int arraySize = _random.Next(5, 50); // Генерируем случайный размер массива
                    var array = GenerateRandomArray(arraySize); // Генерируем случайный массив

                    var model = new SortArrayObservableModel
                    {
                        ArrayData = ParserService.ParseDoubleArrayToString(array), // Преобразуем массив в строку
                        CreatedAt = DateTime.Now // Устанавливаем дату создания
                    };
                    model.Save(); // Сохраняем модель в базу данных
                }

                stopwatch.Stop(); // Останавливаем таймер
                TestContext.WriteLine($"Успешно добавлено {arrayCount} массивов. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
            catch (Exception ex)
            {
                stopwatch.Stop(); // Останавливаем таймер в случае ошибки
                Assert.Fail($"Ошибка при добавлении {arrayCount} массивов: {ex.Message}. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
        }

        /// <summary>
        /// Тестирует добавление 1000 массивов в базу данных.
        /// </summary>
        [Test]
        public void TestAdd1000Arrays()
        {
            const int arrayCount = 1000; // Число массивов для добавления
            var stopwatch = Stopwatch.StartNew(); // Начинаем отсчет времени

            try
            {
                // Добавляем 1000 случайных массивов в базу данных
                for (int i = 0; i < arrayCount; i++)
                {
                    int arraySize = _random.Next(5, 50); // Генерируем случайный размер массива
                    var array = GenerateRandomArray(arraySize); // Генерируем случайный массив

                    var model = new SortArrayObservableModel
                    {
                        ArrayData = ParserService.ParseDoubleArrayToString(array), // Преобразуем массив в строку
                        CreatedAt = DateTime.Now // Устанавливаем дату создания
                    };
                    model.Save(); // Сохраняем модель в базу данных
                }

                stopwatch.Stop(); // Останавливаем таймер
                TestContext.WriteLine($"Успешно добавлено {arrayCount} массивов. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
            catch (Exception ex)
            {
                stopwatch.Stop(); // Останавливаем таймер в случае ошибки
                Assert.Fail($"Ошибка при добавлении {arrayCount} массивов: {ex.Message}. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
        }

        /// <summary>
        /// Тестирует добавление 10000 массивов в базу данных.
        /// </summary>
        [Test]
        public void TestAdd10000Arrays()
        {
            const int arrayCount = 10000; // Число массивов для добавления
            var stopwatch = Stopwatch.StartNew(); // Начинаем отсчет времени

            try
            {
                // Добавляем 10000 случайных массивов в базу данных
                for (int i = 0; i < arrayCount; i++)
                {
                    int arraySize = _random.Next(5, 50); // Генерируем случайный размер массива
                    var array = GenerateRandomArray(arraySize); // Генерируем случайный массив

                    var model = new SortArrayObservableModel
                    {
                        ArrayData = ParserService.ParseDoubleArrayToString(array), // Преобразуем массив в строку
                        CreatedAt = DateTime.Now // Устанавливаем дату создания
                    };
                    model.Save(); // Сохраняем модель в базу данных
                }

                stopwatch.Stop(); // Останавливаем таймер
                TestContext.WriteLine($"Успешно добавлено {arrayCount} массивов. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
            catch (Exception ex)
            {
                stopwatch.Stop(); // Останавливаем таймер в случае ошибки
                Assert.Fail($"Ошибка при добавлении {arrayCount} массивов: {ex.Message}. Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
            }
        }

        /// <summary>
        /// Тестирует загрузку и сортировку всех массивов из базы данных.
        /// </summary>
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void TestLoadAndSortAllArrays(int totalArrays)
        {
            const int batchSize = 1000;
            int remainingArrays = totalArrays;

            // Добавляем массивы в базу данных
            while (remainingArrays > 0)
            {
                int currentBatchSize = Math.Min(batchSize, remainingArrays);
                for (int i = 0; i < currentBatchSize; i++)
                {
                    int arraySize = _random.Next(5, 50); // Генерируем случайный размер массива
                    var array = GenerateRandomArray(arraySize); // Генерируем случайный массив
                    var model = new SortArrayObservableModel
                    {
                        ArrayData = ParserService.ParseDoubleArrayToString(array), // Преобразуем массив в строку
                        CreatedAt = DateTime.Now // Устанавливаем дату создания
                    };
                    model.Save(); // Сохраняем модель в базу данных
                }

                remainingArrays -= currentBatchSize;
                TestContext.WriteLine($"Добавлено {currentBatchSize} массивов. Осталось: {remainingArrays}");
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                const int arraysToSort = 100; // Сортируем только 100 случайных массивов
                var context = DbContextSingleton.Instance;
                var allArrays = context.Arrays.ToList(); // Загружаем все массивы из базы
                var selectedArrays = allArrays.OrderBy(x => _random.Next()).Take(arraysToSort).ToList(); // Выбираем случайные массивы

                // Сортируем выбранные массивы
                foreach (var arrayModel in selectedArrays)
                {
                    var observableModel = new SortArrayObservableModel(arrayModel);
                    var array = ParserService.ParseStringToDoubleArray(observableModel.ArrayData);

                    // Сортируем массив с помощью MergeSort
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
        /// <summary>
        /// Тестирует загрузку и удаление всех массивов из базы данных.
        /// </summary>
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
                TestContext.WriteLine($"Óñïåøíî î÷èùåíà áàçà äàííûõ ñ {totalArrays} çàïèñÿìè. Âðåìÿ âûïîëíåíèÿ: {stopwatch.ElapsedMilliseconds} ìñ");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Assert.Fail($"Îøèáêà ïðè î÷èñòêå áàçû äàííûõ ñ {totalArrays} çàïèñÿìè: {ex.Message}. Âðåìÿ âûïîëíåíèÿ: {stopwatch.ElapsedMilliseconds} ìñ");
            }
        }

    }
}
