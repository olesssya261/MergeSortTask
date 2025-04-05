using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MergeSort.Model.ObservableModels;

namespace MergeSort.Service
{
    public static class TxtExplorer
    {
        /// <summary>
        /// Экспортирует массив и результат сортировки в файл в текстовый файл.
        /// </summary>
        public static void ExportResultsToTxt(SortArrayObservableModel arrayModel, string filePath)
        {
            
                try
                {
                // Создаем или перезаписываем текстовый файл
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Записываем исходный массив
                    if (!string.IsNullOrWhiteSpace(arrayModel.ArrayData)|| !string.IsNullOrWhiteSpace(arrayModel.SortedArrayData)) {
                        writer.WriteLine("Исходный массив:");
                        writer.WriteLine(arrayModel.ArrayData);
                        writer.WriteLine();
                        writer.WriteLine("Метод сортировки:");
                        writer.WriteLine(arrayModel.SortType);
                        writer.WriteLine();
                        writer.WriteLine("Количество перестановок:");
                        writer.WriteLine(arrayModel.Swaps);
                        writer.WriteLine();
                        writer.WriteLine("Количество сравнений:");
                        writer.WriteLine(arrayModel.Comparisons);
                        writer.WriteLine();
                        writer.WriteLine("Итоговый массив:");
                        writer.WriteLine(arrayModel.SortedArrayData);
                        writer.WriteLine();
                    }
                    else
                    {
                        throw new Exception("Исходный или отсортированный массив пуст");
                    }
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Результаты успешно экспортированы в файл: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Ошибка при экспорте результатов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            
        }
        /// <summary>
        /// Экспортирует формулу и параметры задачи в текстовый файл.
        /// </summary>
        public static void ExportParametersToTxt(SortArrayObservableModel arrayModel, string filePath)
        {
            try
                {
                // Создаем или перезаписываем текстовый файл
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Записываем исходный массив
                    if (!string.IsNullOrWhiteSpace(arrayModel.ArrayData))
                    {

                        writer.WriteLine("Исходный массив:");
                        writer.WriteLine(arrayModel.ArrayData);
                    }
                    else
                    {
                        throw new Exception("Исходный массив пуст");
                    }
                }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Исходный массив успешно экспортирован в файл: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
        }
        /// <summary>
        /// Экспортирует формулу и параметры задачи в текстовый файл.
        /// </summary>
        public static SortArrayObservableModel ImportParametersToTxt(string filePath)
        {
            try
            {
                var array = "";
                // Создаем или перезаписываем текстовый файл
                using (StreamReader reader = new StreamReader(filePath))
                {
                    reader.ReadLine(); // Просто читаем и игнорируем

                    // Записываем исходный массив
                    array = reader.ReadToEnd().Replace("\n", " ").Replace("\r", " ");
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Исходный массив успешно импортирован из файл: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                });
                // Создаём новый объект SortArrayObservableModel
                SortArrayObservableModel importedModel = new SortArrayObservableModel
                {
                    ArrayData = array
                };
                return importedModel;
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                });
                SortArrayObservableModel importedModel = new SortArrayObservableModel
                {
                    ArrayData = ""
                };
                return importedModel;
            }
        }
    }
}
