using CommunityToolkit.Mvvm.ComponentModel;
using MergeSort.DbLocator;
using MergeSort.Service;
using Microsoft.EntityFrameworkCore;
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
        DateTime createdAt;

        [ObservableProperty]
        private int idSortArray;

        public SortArrayObservableModel()
        {
            sortArrayModel = new();
        }

        public SortArrayObservableModel(SortArrayModel sortArrayModel)
        {
            this.sortArrayModel = sortArrayModel;
            IdSortArray= sortArrayModel.Id;
            CreatedAt= sortArrayModel.CreatedAt;
            ArrayData = ParserService.ParseDoubleArrayToString(sortArrayModel.ArrayData);
            SortedArrayData = ParserService.ParseDoubleArrayToString(sortArrayModel.SortedArrayData);
            if(sortArrayModel.Swaps.HasValue) Swaps = (uint)sortArrayModel.Swaps;
            if(sortArrayModel.Comparisons.HasValue) Comparisons = (uint)sortArrayModel.Comparisons;
            SortType = sortArrayModel.SortType;
        }
     
        public bool HasChanges => ArrayData != ParserService.ParseDoubleArrayToString(sortArrayModel?.ArrayData) ||
            SortedArrayData != ParserService.ParseDoubleArrayToString(sortArrayModel?.SortedArrayData)
            || Swaps != sortArrayModel?.Swaps
            || Comparisons != sortArrayModel?.Comparisons
            || SortType != sortArrayModel?.SortType;

        public void Save()
        {
            if (!HasChanges)
            {
                MessageBox.Show("Массив не был изменён перед сохранением.",
                          "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                IdSortArray = 0;
            }

            try
            {
                var existingEntity = FindArray();

                 if (existingEntity == null)
                {
                    sortArrayModel = new();
                    UpdateModelFields();
                    sortArrayModel.CreatedAt = DateTime.Now;
                    DbContextSingleton.Instance.Set<SortArrayModel>().Add(sortArrayModel);
                    DbContextSingleton.Instance.SaveChanges();
                    IdSortArray = sortArrayModel.Id;
                }
                else
                {
                    var confirmResult = MessageBox.Show(
                        $"Запись с таким массивом уже существует. Вы уверены, что хотите перезаписать ее данные?",
                        "Подтверждение сохранения",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (confirmResult != MessageBoxResult.No)
                    {
                        UpdateModelFields();
                        existingEntity.UpdateData(sortArrayModel);
                        DbContextSingleton.Instance.SaveChanges();
                    }
                }
                MessageBox.Show("Массив сохранён в БД.",
                           "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (DbUpdateException ex) when (ex.InnerException is Microsoft.Data.Sqlite.SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19) // SQLITE_CONSTRAINT
            {
                MessageBox.Show("Массив с такими данными уже существует в базе данных.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении массива в БД: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Delete()
        {
            try
            {
                var existingEntity = FindArray();

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
        partial void OnArrayDataChanged(string value)
        {
            SortedArrayData = null;
            Swaps = 0;
            Comparisons = 0;
        }
        private void UpdateModelFields()
        {
            sortArrayModel.ArrayData = ParserService.ParseStringToDoubleArray(ArrayData);
            sortArrayModel.SortedArrayData = ParserService.ParseStringToDoubleArray(SortedArrayData);
            sortArrayModel.Swaps = Swaps==0?null:Swaps;
            sortArrayModel.Comparisons = Comparisons == 0 ? null : Comparisons;
            sortArrayModel.SortType = SortType;
        }
        private SortArrayModel? FindArray()
        {
            var query = DbContextSingleton.Instance.Set<SortArrayModel>();

            if (IdSortArray != 0)
            {
                // Сначала ищем по ID (быстро, использует индекс)
                var byId = query.FirstOrDefault(e => e.Id == IdSortArray);
                if (byId != null) return byId;
            }

            // Если не нашли по ID или ID=0, ищем по ArrayData
            var existingEntity = query
                .AsEnumerable() // Переключаемся на клиентскую обработку
                .FirstOrDefault(e => e.ArrayData.SequenceEqual(ParserService.ParseStringToDoubleArray(ArrayData)));
            return existingEntity;
        }
    }
}
