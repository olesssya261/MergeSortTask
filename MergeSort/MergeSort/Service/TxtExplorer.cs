using MergeSort.Model.ObservableModels;
using System.IO;
using System.Text;
using System.Windows;

namespace MergeSort.Service
{
    public static class TxtExplorer
    {
        /// <summary>
        /// Экспортирует исходный массив, отсортированный массив, тип сортировки,
        /// количество перестановок и сравнений в текстовый файл.
        /// </summary>
        /// <param name="arrayModel">Модель, содержащая данные массива и статистику сортировки</param>
        /// <param name="filePath">Путь к файлу, в который будет выполнен экспорт</param>
        public static void ExportResultsToTxt(SortArrayObservableModel arrayModel, string filePath)
        {
            try
            {
                // Создаем или перезаписываем файл UTF-8
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Проверка наличия данных для записи
                    if (!string.IsNullOrWhiteSpace(arrayModel.ArrayData) || !string.IsNullOrWhiteSpace(arrayModel.SortedArrayData))
                    {
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

                // Уведомление об успехе
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Результаты успешно экспортированы в файл: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
            catch (Exception ex)
            {
                // Уведомление об ошибке
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Ошибка при экспорте результатов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        /// <summary>
        /// Экспортирует только исходный массив в текстовый файл.
        /// </summary>
        /// <param name="arrayModel">Модель с исходным массивом</param>
        /// <param name="filePath">Путь к файлу, в который будет выполнен экспорт</param>
        public static void ExportParametersToTxt(SortArrayObservableModel arrayModel, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
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
        /// Импортирует данные из текстового файла и создает объект SortArrayObservableModel.
        /// Предполагается, что в файле находится только строка исходного массива.
        /// </summary>
        /// <param name="filePath">Путь к файлу, из которого будут импортированы данные</param>
        /// <returns>Объект SortArrayObservableModel с импортированным массивом</returns>
        public static SortArrayObservableModel ImportParametersToTxt(string filePath)
        {
            try
            {
                var array = "";

                // Чтение файла
                using (StreamReader reader = new StreamReader(filePath))
                {
                    reader.ReadLine(); // Пропускаем первую строку (например, "Исходный массив:")
                    array = reader.ReadToEnd().Replace("\n", " ").Replace("\r", " ");
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Исходный массив успешно импортирован из файл: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                });

                // Возвращаем модель с импортированными данными
                return new SortArrayObservableModel
                {
                    ArrayData = array
                };
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                });

                // Возвращаем модель с пустыми данными в случае ошибки
                return new SortArrayObservableModel
                {
                    ArrayData = ""
                };
            }
        }
    }
}
