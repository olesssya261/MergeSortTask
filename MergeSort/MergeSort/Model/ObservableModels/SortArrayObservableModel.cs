using CommunityToolkit.Mvvm.ComponentModel;
using MergeSort.DbLocator;
using MergeSort.Service;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace MergeSort.Model.ObservableModels
{
    /// <summary>
    /// Наблюдаемая модель для работы с массивами и их сортировкой.
    /// Предоставляет функционал для сохранения, удаления и обновления данных в базе.
    /// </summary>
    public partial class SortArrayObservableModel : ObservableObject
    {
        /// <summary>
        /// Базовая модель данных для работы с массивом.
        /// </summary>
        private SortArrayModel sortArrayModel;

        /// <summary>
        /// Строковое представление исходного массива данных.
        /// </summary>
        [ObservableProperty]
        string arrayData;

        /// <summary>
        /// Строковое представление отсортированного массива данных.
        /// </summary>
        [ObservableProperty]
        string sortedArrayData;

        /// <summary>
        /// Количество перестановок элементов при сортировке.
        /// </summary>
        [ObservableProperty]
        uint? swaps;

        /// <summary>
        /// Количество сравнений элементов при сортировке.
        /// </summary>
        [ObservableProperty]
        uint? comparisons;

        /// <summary>
        /// Тип алгоритма сортировки.
        /// </summary>
        [ObservableProperty]
        string? sortType;

        /// <summary>
        /// Дата и время создания записи.
        /// </summary>
        [ObservableProperty]
        DateTime createdAt;

        /// <summary>
        /// Инициализирует новый экземпляр класса SortArrayObservableModel.
        /// Создает новую базовую модель для работы.
        /// </summary>
        public SortArrayObservableModel()
        {
            // Инициализация новой модели
            sortArrayModel = new();
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса SortArrayObservableModel на основе существующей модели.
        /// </summary>
        /// <param name="sortArrayModel">Модель данных для инициализации.</param>
        public SortArrayObservableModel(SortArrayModel sortArrayModel)
        {
            // Сохраняем переданную модель
            this.sortArrayModel = sortArrayModel;

            // Копируем основные свойства
            CreatedAt = sortArrayModel.CreatedAt;

            // Конвертируем массивы в строки для отображения
            ArrayData = ParserService.ParseDoubleArrayToString(sortArrayModel.ArrayData);
            SortedArrayData = ParserService.ParseDoubleArrayToString(sortArrayModel.SortedArrayData);

            // Копируем опциональные параметры сортировки
            if (sortArrayModel.Swaps.HasValue) Swaps = sortArrayModel.Swaps;
            if (sortArrayModel.Comparisons.HasValue) Comparisons = sortArrayModel.Comparisons;
            SortType = sortArrayModel.SortType;
        }

        /// <summary>
        /// Сохраняет текущее состояние модели в базе данных.
        /// </summary>
        /// <exception cref="Exception">
        /// Возникает при:
        /// 1. Нарушении уникальности данных (код ошибки 19 SQLite)
        /// 2. Общих ошибках сохранения
        /// </exception>
        public void Save()
        {
            try
            {
                // Пытаемся найти существующую запись с таким же массивом
                var existingEntity = FindArray();

                if (existingEntity == null)
                {
                    // Если запись не найдена - создаем новую
                    UpdateModelFields(sortArrayModel);
                    sortArrayModel.CreatedAt = DateTime.Now; // Устанавливаем текущую дату

                    // Добавляем в контекст и сохраняем
                    DbContextSingleton.Instance.Set<SortArrayModel>().Add(sortArrayModel);
                    DbContextSingleton.Instance.SaveChanges();
                }
                else
                {
                    // Если запись найдена - обновляем ее данные
                    UpdateModelFields(existingEntity);
                    DbContextSingleton.Instance.SaveChanges();
                }

                // Сбрасываем базовую модель после сохранения
                sortArrayModel = new();
            }
            catch (DbUpdateException ex) when (ex.InnerException is Microsoft.Data.Sqlite.SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
            {
                // Обработка нарушения уникальности (массив уже существует)
                throw new Exception("Массив с такими данными уже существует в базе данных.");
            }
            catch (Exception)
            {
                // Общая обработка ошибок сохранения
                throw new Exception("Ошибка при сохранении массива в БД");
            }
        }

        /// <summary>
        /// Удаляет текущий массив из базы данных.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Возникает, если массив для удаления не найден в базе.
        /// </exception>
        public void Delete()
        {
            try
            {
                // Ищем запись для удаления
                var existingEntity = FindArray();

                if (existingEntity == null)
                {
                    throw new InvalidOperationException("Массив для удаления не найден.");
                }

                // Удаляем и сохраняем изменения
                DbContextSingleton.Instance.Set<SortArrayModel>().Remove(existingEntity);
                DbContextSingleton.Instance.SaveChanges();
            }
            catch (Exception ex)
            {
                // Показываем сообщение об ошибке пользователю
                MessageBox.Show($"Ошибка при удалении массива из БД {ex.Message}",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Обработчик изменения свойства ArrayData.
        /// Сбрасывает связанные данные при изменении исходного массива.
        /// </summary>
        /// <param name="value">Новое значение массива.</param>
        partial void OnArrayDataChanged(string value)
        {
            // При изменении исходного массива сбрасываем все производные данные
            SortedArrayData = null;
            SortType = null;
            Swaps = 0;
            Comparisons = 0;
        }

        /// <summary>
        /// Обновляет поля указанной модели на основе текущих значений наблюдаемой модели.
        /// </summary>
        /// <param name="sortArrayModel">Модель для обновления.</param>
        private void UpdateModelFields(SortArrayModel sortArrayModel)
        {
            // Конвертируем строки обратно в массивы чисел
            sortArrayModel.ArrayData = ParserService.ParseStringToDoubleArray(ArrayData);
            sortArrayModel.SortedArrayData = ParserService.ParseStringToDoubleArray(SortedArrayData);

            // Копируем параметры сортировки
            sortArrayModel.Swaps = Swaps;
            sortArrayModel.Comparisons = Comparisons;
            sortArrayModel.SortType = SortType;
        }

        /// <summary>
        /// Находит запись в базе данных по текущему значению массива.
        /// </summary>
        /// <returns>
        /// Найденная модель массива или null, если совпадений не найдено.
        /// </returns>
        private SortArrayModel? FindArray()
        {
            // Получаем все записи из БД и ищем совпадение по массиву
            // Используем AsEnumerable() для клиентской обработки сравнения массивов
            var existingEntity = DbContextSingleton.Instance.Set<SortArrayModel>()
                .AsEnumerable()
                .FirstOrDefault(e => e.ArrayData.SequenceEqual(ParserService.ParseStringToDoubleArray(ArrayData)));

            return existingEntity;
        }
    }
}