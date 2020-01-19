using System;
using DAL_SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL_SqlServer
{
    public partial class ntpContext : DbContext
    {
        public ntpContext()
        {
        }

        public ntpContext(DbContextOptions<ntpContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<EconomicIndicatorEvents> EconomicIndicatorEvents { get; set; }
        public virtual DbSet<EconomicIndicators> EconomicIndicators { get; set; }
        public virtual DbSet<Prices> Prices { get; set; }
        public virtual DbSet<Symbols> Symbols { get; set; }
        public virtual DbSet<VwEconomicIndicators> VwEconomicIndicators { get; set; }
        public virtual DbSet<VwIndicatorCountry> VwIndicatorCountry { get; set; }
        public virtual DbSet<VwPrices> VwPrices { get; set; }
        public virtual DbSet<IndicatorData> IndicatorData { get; set; }
        public virtual DbSet<vwCountryIndicator> vwCountryIndicator { get; set; }
        public virtual DbSet<IndicatorDataCcyName> IndicatorDataCcyName { get; set; }
        public virtual DbSet<IndicatorDataScrapeHistory> IndicatorDataScrapeHistory { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=TOM-HPENVY-16;Initial Catalog=ntp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vwCountryIndicator>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(3);
            });

            modelBuilder.Entity<EconomicIndicatorEvents>(entity =>
            {
                entity.HasKey(e => e.Eieid);

                entity.Property(e => e.Eieid).HasColumnName("EIEId");

                entity.Property(e => e.Actual).HasMaxLength(128);

                entity.Property(e => e.EventDateTime).HasColumnType("datetime");

                entity.Property(e => e.EventTime).HasMaxLength(50);

                entity.Property(e => e.Forecast).HasMaxLength(128);

                entity.Property(e => e.Impact).HasMaxLength(128);

                entity.Property(e => e.Previous).HasMaxLength(128);

                entity.Property(e => e.Revised).HasMaxLength(128);

                entity.HasOne(d => d.Indicator)
                    .WithMany(p => p.EconomicIndicatorEvents)
                    .HasForeignKey(d => d.IndicatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EconomicIndicatorEvents_EconomicIndicators");
            });

            modelBuilder.Entity<EconomicIndicators>(entity =>
            {
                entity.HasKey(e => e.IndicatorId);

                entity.Property(e => e.IndicatorName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.EconomicIndicators)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EconomicIndicators_Countries");
            });

            modelBuilder.Entity<Prices>(entity =>
            {
                entity.HasKey(e => e.PriceId);

                entity.Property(e => e.Ac).HasColumnName("AC");

                entity.Property(e => e.Ah).HasColumnName("AH");

                entity.Property(e => e.Al).HasColumnName("AL");

                entity.Property(e => e.Ao).HasColumnName("AO");

                entity.Property(e => e.Bc).HasColumnName("BC");

                entity.Property(e => e.Bh).HasColumnName("BH");

                entity.Property(e => e.Bl).HasColumnName("BL");

                entity.Property(e => e.Bo).HasColumnName("BO");

                entity.Property(e => e.PriceDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Symbol)
                    .WithMany(p => p.Prices)
                    .HasForeignKey(d => d.SymbolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Prices_Symbols");
            });

            modelBuilder.Entity<Symbols>(entity =>
            {
                entity.HasKey(e => e.SymbolId);

                entity.Property(e => e.SymbolId).ValueGeneratedNever();

                entity.Property(e => e.SymbolCode)
                    .IsRequired()
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<VwEconomicIndicators>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwEconomicIndicators");

                entity.Property(e => e.Actual).HasMaxLength(128);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.Eieid).HasColumnName("EIEId");

                entity.Property(e => e.EventDateTime).HasColumnType("datetime");

                entity.Property(e => e.Forecast).HasMaxLength(128);

                entity.Property(e => e.Impact).HasMaxLength(128);

                entity.Property(e => e.IndicatorName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Previous).HasMaxLength(128);

                entity.Property(e => e.Revised).HasMaxLength(128);
            });

            modelBuilder.Entity<VwIndicatorCountry>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwIndicatorCountry");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.IndicatorName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<VwPrices>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwPrices");

                entity.Property(e => e.PriceDateTime).HasColumnType("datetime");

                entity.Property(e => e.SymbolCode)
                    .IsRequired()
                    .HasMaxLength(6);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
