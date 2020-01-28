using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace webapiDatabaseFirst.Models
{
    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext()
        {
        }

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Configuration> Configuration { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<EconomicIndicatorEvents> EconomicIndicatorEvents { get; set; }
        public virtual DbSet<EconomicIndicators> EconomicIndicators { get; set; }
        public virtual DbSet<IndicatorData> IndicatorData { get; set; }
        public virtual DbSet<IndicatorDataCcyName> IndicatorDataCcyName { get; set; }
        public virtual DbSet<IndicatorDataScrapeHistory> IndicatorDataScrapeHistory { get; set; }
        public virtual DbSet<Prices> Prices { get; set; }
        public virtual DbSet<Symbols> Symbols { get; set; }
        public virtual DbSet<VwCountryIndicator> VwCountryIndicator { get; set; }
        public virtual DbSet<VwEconomicIndicators> VwEconomicIndicators { get; set; }
        public virtual DbSet<VwIndicatorCountry> VwIndicatorCountry { get; set; }
        public virtual DbSet<VwPrices> VwPrices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=TOM-HPENVY-16;Database=ntp;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<EconomicIndicatorEvents>(entity =>
            {
                entity.HasOne(d => d.Indicator)
                    .WithMany(p => p.EconomicIndicatorEvents)
                    .HasForeignKey(d => d.IndicatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EconomicIndicatorEvents_EconomicIndicators");
            });

            modelBuilder.Entity<EconomicIndicators>(entity =>
            {
                entity.HasOne(d => d.Country)
                    .WithMany(p => p.EconomicIndicators)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EconomicIndicators_Countries");
            });

            modelBuilder.Entity<IndicatorData>(entity =>
            {
                entity.HasIndex(e => e.Currency)
                    .HasName("IX_IndicatorData_currency");

                entity.HasIndex(e => e.EventId)
                    .HasName("IX_IndicatorData_eventid");

                entity.HasIndex(e => e.ReleaseDateTime);
            });

            modelBuilder.Entity<IndicatorDataCcyName>(entity =>
            {
                entity.HasIndex(e => e.Currency)
                    .HasName("IX_IndicatorDataCcyName_currency");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<IndicatorDataScrapeHistory>(entity =>
            {
                entity.HasIndex(e => e.ScrapeDate)
                    .HasName("IX_IndicatorDataScrapeHistory");
            });

            modelBuilder.Entity<Symbols>(entity =>
            {
                entity.HasKey(e => e.SymbolId)
                    .HasName("PK_Symbols_1");
            });

            modelBuilder.Entity<VwCountryIndicator>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwCountryIndicator");
            });

            modelBuilder.Entity<VwEconomicIndicators>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwEconomicIndicators");
            });

            modelBuilder.Entity<VwIndicatorCountry>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwIndicatorCountry");
            });

            modelBuilder.Entity<VwPrices>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwPrices");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
