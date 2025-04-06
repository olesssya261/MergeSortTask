using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MergeSort.DbLocator;
using MergeSort.Enum;
using MergeSort.Model.ObservableModels;
using MergeSort.Service;
using SortedModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace MergeSort.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        // Событие, которое уведомляет об открытии массива
        public event EventHandler openArray;

        [ObservableProperty]
        private bool sortedArrayFlag = false; // Флаг, показывающий, отсортирован ли массив

        [ObservableProperty]
        ObservableCollection<SortArrayObservableModel> sortedArrayModels; // Коллекция отсортированных массивов

        [ObservableProperty]
        ObservableCollection<SortArrayObservableModel> initialArrayModels; // Коллекция исходных массивов

        [ObservableProperty]
        SortArrayObservableModel arrayModel = new(); // Модель для работы с данным массивом

        // Словарь с методами сортировки
        [ObservableProperty]
        private Dictionary<string, MethodsEnum> methods = new Dictionary<string, MethodsEnum>
        {
            { "Сортировка слиянием",  MethodsEnum.MergeSort},
            { "Сортировка пузырьком",  MethodsEnum.BubleSort},
            { "Быстрая сортировка",  MethodsEnum.Quick}
        };

        [ObservableProperty]
        private MethodsEnum selectedMethod; // Выбранный метод сортировки

        public MainViewModel() { }

        /// <summary>
        /// Заполняет окно БД.
        /// Загружает отсортированные и исходные массивы.
        /// </summary>
        [RelayCommand]
        private void OpenDbMenu()
        {
            // Загружаем массивы, которые уже отсортированы
            SortedArrayModels = new ObservableCollection<SortArrayObservableModel>(DbContextSingleton.Instance.Arrays
                .Where(e => !string.IsNullOrEmpty(e.SortType))
                .Select(array => new SortArrayObservableModel(array)
                ));

            // Загружаем массивы, которые еще не отсортированы
            InitialArrayModels = new ObservableCollection<SortArrayObservableModel>(DbContextSingleton.Instance.Arrays
                .Where(e => string.IsNullOrEmpty(e.SortType))
                .Select(array => new SortArrayObservableModel(array)
                ));
        }

        /// <summary>
        /// Выполняет сортировку массива в зависимости от выбранного метода.
        /// </summary>
        [RelayCommand]
        private void Sort()
        {
            try
            {
                // Преобразуем строку в массив чисел
                double[] array = ParserService.ParseStringToDoubleArray(ArrayModel.ArrayData);

                // Устанавливаем тип сортировки на основе выбранного метода
                ArrayModel.SortType = Methods.FirstOrDefault(x => x.Value == SelectedMethod).Key;

                // Выполняем сортировку в зависимости от выбранного метода
                switch (SelectedMethod)
                {
                    case MethodsEnum.MergeSort:
                        var mergeResult = SortModel.MergeSort(array);
                        ArrayModel.SortedArrayData = ParserService.ParseDoubleArrayToString(mergeResult.sortedArray);
                        ArrayModel.Swaps = mergeResult.swaps;
                        ArrayModel.Comparisons = mergeResult.comparisons;
                        break;
                    case MethodsEnum.BubleSort:
                        var bubbleResult = SortModel.BubbleSort(array);
                        ArrayModel.SortedArrayData = ParserService.ParseDoubleArrayToString(bubbleResult.sortedArray);
                        ArrayModel.Swaps = bubbleResult.swaps;
                        ArrayModel.Comparisons = bubbleResult.comparisons;
                        break;
                    case MethodsEnum.Quick:
                        var quickResult = SortModel.QuickSort(array);
                        ArrayModel.SortedArrayData = ParserService.ParseDoubleArrayToString(quickResult.sortedArray);
                        ArrayModel.Swaps = quickResult.swaps;
                        ArrayModel.Comparisons = quickResult.comparisons;
                        break;
                    default:
                        throw new Exception("Не выбран метод сортировки");
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки при сортировке показываем сообщение об ошибке
                MessageBox.Show($"Ошибка при сортировке массива: {ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Открывает выбранный массив для дальнейшего редактирования.
        /// </summary>
        /// <param name="clickedItem">Выбранный элемент массива</param>
        [RelayCommand]
        private void OpenArray(SortArrayObservableModel clickedItem)
        {
            if (clickedItem != null)
            {
                ArrayModel = clickedItem; // Устанавливаем выбранный элемент как текущую модель
                openArray?.Invoke(this, new EventArgs()); // Вызываем событие открытия массива
            }
        }

        /// <summary>
        /// Импортирует параметры массива из Excel файла.
        /// </summary>
        [RelayCommand]
        private void ImportParametersFromExel()
        {
            // Задаем фильтр для выбора файлов Excel
            string filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Open, filter, "Выберите файл с параметрами");

            if (!string.IsNullOrEmpty(filePath))
            {
                // Импортируем данные из выбранного Excel файла
                ArrayModel = ExcelExplorer.ImportArrayFromExcel(filePath);
            }
        }

        /// <summary>
        /// Импортирует параметры массива из текстового файла.
        /// </summary>
        [RelayCommand]
        private void ImportParametersFromTxt()
        {
            // Задаем фильтр для выбора текстовых файлов
            string filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Open, filter, "Выберите файл с параметрами");

            if (!string.IsNullOrEmpty(filePath))
            {
                // Импортируем данные из выбранного текстового файла
                ArrayModel = TxtExplorer.ImportParametersToTxt(filePath);
            }
        }

        /// <summary>
        /// Экспортирует параметры массива в файл Excel.
        /// </summary>
        [RelayCommand]
        private void ExportParametersToExel()
        {
            // Задаем фильтр для выбора файла Excel
            string filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Save, filter, $"Параметры_{DateTime.Now:yyyyMMdd_HHmmss}", "Сохранить исходные данные в Excel");

            if (!string.IsNullOrEmpty(filePath))
            {
                if (ArrayModel != null && !string.IsNullOrEmpty(ArrayModel.ArrayData))
                {
                    // Экспортируем данные в файл Excel
                    ExcelExplorer.ExportParametersToExcel(ArrayModel, filePath);
                }
                else
                {
                    // Если исходный массив пуст, выводим предупреждение
                    MessageBox.Show("Исходный массив пуст. Пожалуйста, заполните данные перед экспортом.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Экспортирует параметры массива в текстовый файл.
        /// </summary>
        [RelayCommand]
        private void ExportParametersToTxt()
        {
            // Задаем фильтр для выбора текстовых файлов
            string filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Save, filter, $"Параметры_{DateTime.Now:yyyyMMdd_HHmmss}", "Сохранить исходные данные в Excel");

            if (!string.IsNullOrEmpty(filePath))
            {
                if (ArrayModel != null && !string.IsNullOrEmpty(ArrayModel.ArrayData))
                {
                    // Экспортируем данные в текстовый файл
                    TxtExplorer.ExportParametersToTxt(ArrayModel, filePath);
                }
                else
                {
                    // Если исходный массив пуст, выводим предупреждение
                    MessageBox.Show("Исходный массив пуст. Пожалуйста, заполните данные перед экспортом.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Экспортирует результаты сортировки в текстовый файл.
        /// </summary>
        [RelayCommand]
        private void ExportResultsToTxt()
        {
            // Задаем фильтр для выбора текстовых файлов
            string filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Save, filter, $"Результаты_{DateTime.Now:yyyyMMdd_HHmmss}", "Сохранить итоговые данные в TXT");

            if (!string.IsNullOrEmpty(filePath))
            {
                if (ArrayModel != null && !string.IsNullOrEmpty(ArrayModel.ArrayData))
                {
                    if (string.IsNullOrEmpty(ArrayModel.SortedArrayData))
                    {
                        // Если массив еще не отсортирован, выводим предупреждение
                        MessageBox.Show("Массив ещё не отсортирован. Пожалуйста, выполните сортировку перед экспортом итоговых данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    // Экспортируем результаты в текстовый файл
                    TxtExplorer.ExportResultsToTxt(ArrayModel, filePath);
                }
                else
                {
                    // Если исходный массив пуст, выводим предупреждение
                    MessageBox.Show("Исходный массив пуст. Пожалуйста, заполните данные перед экспортом.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Экспортирует результаты сортировки в Excel.
        /// </summary>
        [RelayCommand]
        private void ExportResultsToExel()
        {
            // Задаем фильтр для выбора файла Excel
            string filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Save, filter, $"Результаты_{DateTime.Now:yyyyMMdd_HHmmss}", "Сохранить итоговые данные в Excel");

            if (!string.IsNullOrEmpty(filePath))
            {
                if (ArrayModel != null && !string.IsNullOrEmpty(ArrayModel.ArrayData))
                {
                    if (string.IsNullOrEmpty(ArrayModel.SortedArrayData))
                    {
                        // Если массив еще не отсортирован, выводим предупреждение
                        MessageBox.Show("Массив ещё не отсортирован. Пожалуйста, выполните сортировку перед экспортом итоговых данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    // Экспортируем результаты в Excel
                    ExcelExplorer.ExportResultsToExcel(ArrayModel, filePath);
                }
                else
                {
                    // Если исходный массив пуст, выводим предупреждение
                    MessageBox.Show("Исходный массив пуст. Пожалуйста, заполните данные перед экспортом.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Сохраняет текущий массив в базе данных.
        /// </summary>
        [RelayCommand]
        private void SaveArray()
        {
            try
            {
                ArrayModel.Save(); // Сохраняем массив в базе данных
                MessageBox.Show("Массив сохранён в БД.",
                           "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // В случае ошибки при сохранении выводим сообщение
                MessageBox.Show($"Ошибка при сохранении массива в БД: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Удаляет выбранный массив из базы данных.
        /// </summary>
        [RelayCommand]
        private void DeleteArray(SortArrayObservableModel sortArrayObservableModel)
        {
            sortArrayObservableModel.Delete(); // Удаляем выбранный массив из базы данных
            OpenDbMenu(); // Обновляем данные в базе данных
        }

        /// <summary>
        /// Открывает окно "О программе".
        /// </summary>
        [RelayCommand]
        void OpenAboutWindow()
        {
            var aboutWindow = new AboutWindow(new AboutViewModel());
            aboutWindow.Show();
            aboutWindow.Closing += AboutWindowClosing;
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Hide();
        }

        // Обработчик закрытия окна "О программе"
        private void AboutWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mainWindow != null)
            {
                mainWindow.Show();
                mainWindow.Activate();
            }
        }
    }
}
