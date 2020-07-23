using System;
using DssMultipleCriteriaAnalysis.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DssMultipleCriteriaAnalysis
{
    public partial class DssDatasetContext : DbContext
    {
        public DssDatasetContext()
        {
        }

        public DssDatasetContext(DbContextOptions<DssDatasetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Exchanges> Exchanges { get; set; }
        public virtual DbSet<Final> Final { get; set; }
        public virtual DbSet<Total> Total { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=DssDataset; Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CountryName).HasMaxLength(100);
            });

            modelBuilder.Entity<Exchanges>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Export).HasColumnType("numeric(18, 8)");

                entity.Property(e => e.Import).HasColumnType("numeric(18, 8)");
            });

            modelBuilder.Entity<Final>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Weight).HasColumnType("numeric(18, 8)");
            });

            modelBuilder.Entity<Total>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Weight).HasColumnType("numeric(18, 8)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
