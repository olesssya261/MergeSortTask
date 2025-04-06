using MergeSort.Enum;
using Microsoft.Win32;

namespace MergeSort.Service
{
    public static class FileProvider
    {
        /// <summary>
        /// Открывает диалоговое окно для выбора или сохранения файла.
        /// </summary>
        /// <param name="mode">Режим работы: Open для чтения, Save для сохранения</param>
        /// <param name="filter">Фильтр расширений (например, "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*")</param>
        /// <param name="defaultFileName">Имя файла по умолчанию (только для режима Save)</param>
        /// <param name="title">Заголовок диалогового окна</param>
        /// <returns>Путь к выбранному файлу или null, если выбор отменён</returns>
        public static string GetFilePath(FileMode mode, string filter, string defaultFileName = "", string title = "Выберите файл")
        {
            // Проверяем, что фильтр задан
            if (string.IsNullOrEmpty(filter))
                throw new ArgumentException("Параметр filter не может быть пустым.");

            // Выбираем действие в зависимости от режима (открытие или сохранение файла)
            switch (mode)
            {
                case FileMode.Open:
                    return OpenFileDialog(filter, title); // Открытие файла
                case FileMode.Save:
                    return SaveFileDialog(filter, defaultFileName, title); // Сохранение файла
                default:
                    throw new ArgumentException("Недопустимый режим. Используйте FileMode.Open или FileMode.Save.");
            }
        }

        /// <summary>
        /// Открывает диалоговое окно для выбора файла.
        /// </summary>
        private static string OpenFileDialog(string filter, string title)
        {
            // Создаем диалоговое окно для открытия файла
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = filter, // Применяем фильтр расширений
                Title = title, // Устанавливаем заголовок окна
                Multiselect = false // Разрешаем выбор только одного файла
            };

            // Показываем диалог и возвращаем путь выбранного файла, если пользователь подтвердил выбор
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return null; // Возвращаем null, если выбор отменён
        }

        /// <summary>
        /// Открывает диалоговое окно для сохранения файла.
        /// </summary>
        private static string SaveFileDialog(string filter, string defaultFileName, string title)
        {
            // Создаем диалоговое окно для сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = filter, // Применяем фильтр расширений
                Title = title, // Устанавливаем заголовок окна
                FileName = defaultFileName, // Имя файла по умолчанию (для сохранения)
                DefaultExt = GetDefaultExtension(filter), // Устанавливаем расширение по умолчанию
                OverwritePrompt = true // Показывать предупреждение, если файл уже существует
            };

            // Показываем диалог и возвращаем путь сохранённого файла, если пользователь подтвердил выбор
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null; // Возвращаем null, если выбор отменён
        }

        /// <summary>
        /// Извлекает расширение по умолчанию из фильтра.
        /// </summary>
        private static string GetDefaultExtension(string filter)
        {
            // Разделяем фильтр на части
            string[] parts = filter.Split('|');
            if (parts.Length > 1)
            {
                // Получаем первое расширение из фильтра (например, *.xlsx)
                string firstExtension = parts[1].Split(';')[0].Replace("*", "").Trim();
                return firstExtension; // Возвращаем расширение
            }
            return ""; // Если расширение не указано, возвращаем пустую строку
        }
    }
}
