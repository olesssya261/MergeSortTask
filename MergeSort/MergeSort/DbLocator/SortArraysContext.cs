using Microsoft.EntityFrameworkCore;

namespace MergeSort.DbLocator
{
    /// <summary>
    /// Контекст базы данных для работы с массивами.
    /// </summary>
    public class SortArraysContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр контекста базы данных.
        /// </summary>
        public SortArraysContext()
        {
            //Создаёт БД если такова не была создана
            Database.EnsureCreated();
        }

        /// <summary>
        /// Коллекция массивов в базе данных.
        /// </summary>
        public DbSet<SortArrayModel> Arrays { get; set; }

        /// <summary>
        /// Настраивает параметры подключения к базе данных.
        /// </summary>
        /// <param name="optionsBuilder">Построитель опций контекста.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mydatabase.db");
        }

        /// <summary>
        /// Настраивает модель базы данных.
        /// </summary>
        /// <param name="modelBuilder">Построитель модели.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SortArrayModel>(entity =>
            {
                //Создаёт первичный ключ по ID
                entity.HasKey(e => e.Id);
                //Создаёт альтернативный ключ по ArrayDataBlob(Исходный массив)
                entity.HasIndex(e => e.ArrayDataBlob)
                      .IsUnique();
            });
        }
    }
}