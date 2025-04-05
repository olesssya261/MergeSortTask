using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (string.IsNullOrEmpty(filter))
                throw new ArgumentException("Параметр filter не может быть пустым.");

            switch (mode)
            {
                case FileMode.Open:
                    return OpenFileDialog(filter, title);
                case FileMode.Save:
                    return SaveFileDialog(filter, defaultFileName, title);
                default:
                    throw new ArgumentException("Недопустимый режим. Используйте FileMode.Open или FileMode.Save.");
            }
        }

        /// <summary>
        /// Открывает диалоговое окно для выбора файла.
        /// </summary>
        private static string OpenFileDialog(string filter, string title)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = filter,
                Title = title,
                Multiselect = false // Разрешаем выбор только одного файла
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        /// <summary>
        /// Открывает диалоговое окно для сохранения файла.
        /// </summary>
        private static string SaveFileDialog(string filter, string defaultFileName, string title)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                Title = title,
                FileName = defaultFileName,
                DefaultExt = GetDefaultExtension(filter), // Устанавливаем расширение по умолчанию
                OverwritePrompt = true // Предупреждать, если файл существует
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        /// <summary>
        /// Извлекает расширение по умолчанию из фильтра.
        /// </summary>
        private static string GetDefaultExtension(string filter)
        {
            string[] parts = filter.Split('|');
            if (parts.Length > 1)
            {
                string firstExtension = parts[1].Split(';')[0].Replace("*", "").Trim();
                return firstExtension;
            }
            return "";
        }
    }
}
