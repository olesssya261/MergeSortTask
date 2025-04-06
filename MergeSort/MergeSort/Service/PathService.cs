namespace MergeSort.Service
{
    public static class PathService
    {
        /// <summary>
        /// Возвращает путь к текущей директории приложения с возможным добавлением подкаталога.
        /// </summary>
        /// <param name="additionPath">Дополнительный путь, который будет добавлен к текущей директории (например: "data\\output")</param>
        /// <returns>Строка с полным путем к текущей папке и подкаталогу</returns>
        /// <exception cref="Exception">Выбрасывается, если невозможно получить текущую директорию</exception>
        public static string GetCurentFolderPath(string additionPath = "")
        {
            try
            {
                // Получаем путь к рабочей директории и добавляем к нему дополнительный путь (если задан)
                return $"{Environment.CurrentDirectory}\\{additionPath}";
            }
            catch (Exception)
            {
                // Если по каким-то причинам не удалось получить путь, выбрасываем исключение
                throw new Exception("Невозможно установить путь к используемой директории");
            }
        }
    }
}
