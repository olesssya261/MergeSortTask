using Microsoft.EntityFrameworkCore;

namespace MergeSort.DbLocator
{
    public class SortArraysContext : DbContext
    {
        public SortArraysContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<SortArrayModel> Arrays { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mydatabase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SortArrayModel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.ArrayDataBlob)
                      .IsUnique();
            });
        }
    }
}