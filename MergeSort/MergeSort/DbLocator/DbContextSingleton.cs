namespace MergeSort.DbLocator
{
    /// <summary>
    /// Предоставляет глобальную точку доступа к единственному экземпляру контекста базы данных
    /// с поддержкой пересоздания при освобождении ресурсов для однопоточного приложения.
    /// </summary>
    public static class DbContextSingleton
    {
        // Поле для хранения экземпляра контекста
        private static SortArraysContext _context;

        /// <summary>
        /// Статический конструктор для инициализации начального экземпляра
        /// </summary>
        static DbContextSingleton()
        {
            InitializeContext();
        }

        /// <summary>
        /// Получает глобальный экземпляр контекста базы данных.
        /// Если контекст был освобожден, создает новый экземпляр.
        /// </summary>
        /// <returns>Экземпляр SortArraysContext</returns>
        public static SortArraysContext Instance
        {
            get
            {
                // Проверяем, был ли контекст освобожден или не создан
                if (_context == null || _context.Database == null)
                {
                    InitializeContext();
                }
                return _context;
            }
        }

        /// <summary>
        /// Инициализирует или переинициализирует контекст базы данных
        /// </summary>
        private static void InitializeContext()
        {
            var newContext = new SortArraysContext();
            newContext.Database.EnsureCreated();
            _context = newContext;
        }

        /// <summary>
        /// Освобождает ресурсы текущего контекста базы данных.
        /// После вызова будет создан новый экземпляр при следующем обращении.
        /// </summary>
        public static void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null; // Устанавливаем null для создания нового экземпляра
            }
        }

        /// <summary>
        /// Проверяет, инициализирован ли текущий контекст
        /// </summary>
        /// <returns>true, если контекст существует и не освобожден; false в противном случае</returns>
        public static bool IsContextInitialized()
        {
            return _context != null && _context.Database != null;
        }
    }
}