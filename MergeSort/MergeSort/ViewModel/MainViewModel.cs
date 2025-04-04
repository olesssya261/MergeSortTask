using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MergeSort.DbLocator;
using MergeSort.Enum;
using MergeSort.Model.ObservableModels;
using MergeSort.Service;
using Microsoft.EntityFrameworkCore;
using SortedModel;

namespace MergeSort.ViewModel
{
    public partial class MainViewModel:ObservableObject
    {
        [ObservableProperty]
        private bool sortedArrayFlag =false;

        [ObservableProperty]
        ObservableCollection<SortArrayObservableModel> sortedArrayModels;
        [ObservableProperty]
        ObservableCollection<SortArrayObservableModel> initialArrayModels;

        [ObservableProperty]
        SortArrayObservableModel selectedArrayModel;

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

        public MainViewModel(){}

        [RelayCommand]
        private void OpenDbMenu()
        {
            SortedArrayModels = new ObservableCollection<SortArrayObservableModel>(DbContextSingleton.Instance.Arrays
                .Where(e=>!string.IsNullOrEmpty(e.SortType))
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
                switch (SelectedMethod) {
                    case MethodsEnum.MergeSort:
                        var mergeResult = SortModel.MergeSort(array);
                        ArrayModel.SortedArrayData = ParserService.ParseDoubleArrayToString( mergeResult.sortedArray);
                        ArrayModel.Swaps= mergeResult.swaps;
                        ArrayModel.Comparisons= mergeResult.comparisons;
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
            catch (Exception ex) {
                MessageBox.Show($"Ошибка при сортировке массива: {ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void SaveArray()
        {
            ArrayModel.Save();
        }
        [RelayCommand]
        private void DeleteArray(SortArrayObservableModel sortArrayObservableModel)
        {
            sortArrayObservableModel.Delete();
            OpenDbMenu();
        }

    }
}
