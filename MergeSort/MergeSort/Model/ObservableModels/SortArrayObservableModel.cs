using CommunityToolkit.Mvvm.ComponentModel;
using MergeSort.DbLocator;
using MergeSort.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MergeSort.Model.ObservableModels
{
    public partial class SortArrayObservableModel:ObservableObject
    {
        private SortArrayModel sortArrayModel;
        [ObservableProperty]
        string arrayData;

        [ObservableProperty]
        string sortedArrayData;

        [ObservableProperty]
        uint swaps;

        [ObservableProperty]
        uint comparisons;

        [ObservableProperty]
        string sortType;

        [ObservableProperty]
        private int idSortArray;

        public SortArrayObservableModel()
        {
            sortArrayModel = new();
        }

        public SortArrayObservableModel(SortArrayModel sortArrayModel)
        {
            this.sortArrayModel = sortArrayModel;
            ArrayData = ParserService.ParseDoubleArrayToString(sortArrayModel.ArrayData);
            SortedArrayData = ParserService.ParseDoubleArrayToString(sortArrayModel.SortedArrayData);
            if (sortArrayModel.Swaps.HasValue)
            {
                Swaps = (uint)sortArrayModel.Swaps;
            }
            if (sortArrayModel.Comparisons.HasValue)
            {
                Comparisons = (uint)sortArrayModel.Comparisons;
            }
            SortType = sortArrayModel.SortType;
        }
     
        public bool HasChanges => ArrayData != ParserService.ParseDoubleArrayToString(sortArrayModel.ArrayData) ||
            SortedArrayData != ParserService.ParseDoubleArrayToString(sortArrayModel.SortedArrayData)
            || Swaps != sortArrayModel.Swaps
            || Comparisons != sortArrayModel.Comparisons
            || SortType != sortArrayModel.SortType;

        public void Save()
        {
            if (!HasChanges) return;
            try
            {
                sortArrayModel.ArrayData = ParserService.ParseStringToDoubleArray(ArrayData);
                sortArrayModel.SortedArrayData = ParserService.ParseStringToDoubleArray(SortedArrayData);
                sortArrayModel.Swaps = Swaps;
                sortArrayModel.Comparisons = Comparisons;
                sortArrayModel.SortType = SortType;
                var existingEntity = DbContextSingleton.Instance.Set<SortArrayModel>()
                .Where(e=>e.Id==IdSortArray||e.ArrayData== ParserService.ParseStringToDoubleArray(ArrayData)).FirstOrDefault();

                if (existingEntity == null)
                {
                    DbContextSingleton.Instance.Set<SortArrayModel>().Add(sortArrayModel);
                }
                else
                {
                    DbContextSingleton.Instance.Entry(existingEntity).CurrentValues.SetValues(sortArrayModel);
                    IdSortArray = sortArrayModel.Id;
                }

                DbContextSingleton.Instance.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении массива в БД {ex.Message}",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        public void Delete()
        {
            try
            {
                var existingEntity = DbContextSingleton.Instance.Set<SortArrayModel>()
                 .Where(e => e.Id == IdSortArray || e.ArrayData == ParserService.ParseStringToDoubleArray(ArrayData)).FirstOrDefault();

                if (existingEntity == null)
                {
                    throw new InvalidOperationException("Генератор для удаления не найден.");
                }

                DbContextSingleton.Instance.Set<SortArrayModel>().Remove(existingEntity);

                DbContextSingleton.Instance.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении массива из БД {ex.Message}",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
