using Microsoft.EntityFrameworkCore;
using SortedModel;
using System;

namespace MergeSort.DbLocator
{
    public class SortArraysContext : DbContext
    {
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

                entity.HasAlternateKey(e => e.ArrayDataBlob);
                // Настройка хранения BLOB данных
                entity.Property(e => e.ArrayDataBlob)
                    .IsRequired()
                    .HasColumnType("BLOB");

                entity.Property(e => e.SortedArrayDataBlob)
                    .HasColumnType("BLOB");
            });
        }
    }
}