using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MergeSort.DbLocator;
using MergeSort.Enum;
using MergeSort.Model.ObservableModels;
using MergeSort.Service;
using SortedModel;

namespace MergeSort.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        public event EventHandler openArray;

        [ObservableProperty]
        private bool sortedArrayFlag = false;

        [ObservableProperty]
        ObservableCollection<SortArrayObservableModel> sortedArrayModels;

        [ObservableProperty]
        ObservableCollection<SortArrayObservableModel> initialArrayModels;

        [ObservableProperty]
        SortArrayObservableModel arrayModel = new();

        [ObservableProperty]
        private Dictionary<string, MethodsEnum> methods = new Dictionary<string, MethodsEnum>
            {
                { "Сортировка слиянием",  MethodsEnum.MergeSort},
            {"Сортировка пузырьком",  MethodsEnum.BubleSort},
            {"Быстрая сортировка",  MethodsEnum.Quick}
            };

        [ObservableProperty]
        private MethodsEnum selectedMethod;

        public MainViewModel() { }

        [RelayCommand]
        private void OpenDbMenu()
        {
            SortedArrayModels = new ObservableCollection<SortArrayObservableModel>(DbContextSingleton.Instance.Arrays
                .Where(e => !string.IsNullOrEmpty(e.SortType))
                 .Select(array => new SortArrayObservableModel(array)
                ));
            InitialArrayModels = new ObservableCollection<SortArrayObservableModel>(DbContextSingleton.Instance.Arrays
                .Where(e => string.IsNullOrEmpty(e.SortType))
                 .Select(array => new SortArrayObservableModel(array)
                ));
        }

        [RelayCommand]
        private void Sort()
        {
            try
            {
                double[] array = ParserService.ParseStringToDoubleArray(ArrayModel.ArrayData);
                ArrayModel.SortType = Methods.FirstOrDefault(x => x.Value == SelectedMethod).Key;
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
                MessageBox.Show($"Ошибка при сортировке массива: {ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        [RelayCommand]
        private void OpenArray(SortArrayObservableModel clickedItem)
        {
            if (clickedItem != null)
            {
                ArrayModel = clickedItem;
                openArray?.Invoke(this, new EventArgs());
            }
        }

        [RelayCommand]
       private void ImportParametersFromExel()
        {
            string filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Open, filter, "Выберите файл с параметрами");

            if (!string.IsNullOrEmpty(filePath))
                {
                ArrayModel=ExcelExplorer.ImportArrayFromExcel(filePath);
                }
            
        }

        [RelayCommand]
        private void ImportParametersFromTxt()
        {
            string filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Open, filter, "Выберите файл с параметрами");

            if (!string.IsNullOrEmpty(filePath))
            {
                ArrayModel = TxtExplorer.ImportParametersToTxt(filePath);
            }

        }
       
        [RelayCommand]
        private void ExportParametersToExel()
        {
            string filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Save, filter, $"Параметры_{DateTime.Now:yyyyMMdd_HHmmss}", "Сохранить исходные данные в Excel");

            if (!string.IsNullOrEmpty(filePath))
            {
                if (ArrayModel != null && !string.IsNullOrEmpty(ArrayModel.ArrayData))
                {
                    ExcelExplorer.ExportParametersToExcel(ArrayModel, filePath);
                }
                else
                {
                    MessageBox.Show("Исходный массив пуст. Пожалуйста, заполните данные перед экспортом.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        [RelayCommand]
        private void ExportParametersToTxt()
        {
            string filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Save, filter, $"Параметры_{DateTime.Now:yyyyMMdd_HHmmss}", "Сохранить исходные данные в Excel");

            if (!string.IsNullOrEmpty(filePath))
            {
                if (ArrayModel != null && !string.IsNullOrEmpty(ArrayModel.ArrayData))
                {
                    TxtExplorer.ExportParametersToTxt(ArrayModel, filePath);
                }
                else
                {
                    MessageBox.Show("Исходный массив пуст. Пожалуйста, заполните данные перед экспортом.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        [RelayCommand]
        private void ExportResultsToTxt()
        {
            string filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Save, filter, $"Результаты_{DateTime.Now:yyyyMMdd_HHmmss}", "Сохранить итоговые данные в TXT");

            if (!string.IsNullOrEmpty(filePath))
            {
                if (ArrayModel != null && !string.IsNullOrEmpty(ArrayModel.ArrayData))
                {
                    if (string.IsNullOrEmpty(ArrayModel.SortedArrayData))
                    {
                        MessageBox.Show("Массив ещё не отсортирован. Пожалуйста, выполните сортировку перед экспортом итоговых данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    TxtExplorer.ExportResultsToTxt(ArrayModel, filePath);
                }
                else
                {
                    MessageBox.Show("Исходный массив пуст. Пожалуйста, заполните данные перед экспортом.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        [RelayCommand]
        private void ExportResultsToExel()
        {
            string filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            string filePath = FileProvider.GetFilePath(FileMode.Save, filter, $"Результаты_{DateTime.Now:yyyyMMdd_HHmmss}", "Сохранить итоговые данные в Excel");

            if (!string.IsNullOrEmpty(filePath))
            {
                if (ArrayModel != null && !string.IsNullOrEmpty(ArrayModel.ArrayData))
                {
                    if (string.IsNullOrEmpty(ArrayModel.SortedArrayData))
                    {
                        MessageBox.Show("Массив ещё не отсортирован. Пожалуйста, выполните сортировку перед экспортом итоговых данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    ExcelExplorer.ExportResultsToExcel(ArrayModel, filePath);
                }
                else
                {
                    MessageBox.Show("Исходный массив пуст. Пожалуйста, заполните данные перед экспортом.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        [RelayCommand]
        private void SaveArray()
        {
            try
            {
                ArrayModel.Save();
                MessageBox.Show("Массив сохранён в БД.",
                           "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) {
                MessageBox.Show($"Ошибка при сохранении массива в БД: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        [RelayCommand]
        private void DeleteArray(SortArrayObservableModel sortArrayObservableModel)
        {
            sortArrayObservableModel.Delete();
            OpenDbMenu();
        }
        [RelayCommand]
        void OpenAboutWindow()
        {
            var aboutWindow = new AboutWindow(new AboutViewModel());
            aboutWindow.Show();
            aboutWindow.Closing += AboutWindowClosing;
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Hide();
        }

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
