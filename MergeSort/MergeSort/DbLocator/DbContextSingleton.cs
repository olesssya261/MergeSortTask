namespace MergeSort.DbLocator
{
    public static class DbContextSingleton
    {
        private static readonly Lazy<SortArraysContext> _lazyContext =
            new Lazy<SortArraysContext>(() =>
            {
                var context = new SortArraysContext();
                context.Database.EnsureCreated();
                return context;
            });

        /// <summary>
        /// Получает единственный экземпляр контекста БД
        /// </summary>
        public static SortArraysContext Instance => _lazyContext.Value;

        /// <summary>
        /// Явно освобождает ресурсы контекста (вызывать при завершении приложения)
        /// </summary>
        public static void Dispose()
        {
            if (_lazyContext.IsValueCreated)
            {
                _lazyContext.Value.Dispose();
            }
        }
    }
}