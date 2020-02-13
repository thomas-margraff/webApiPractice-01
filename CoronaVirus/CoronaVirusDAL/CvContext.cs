using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CoronaVirusDAL.Entities;

namespace CoronaVirusDAL
{
    public class CvContext : DbContext
    {
        public virtual DbSet<ScrapeRun> ScrapeRuns { get; set; }
        public virtual DbSet<GeoLocation> GeoLocations { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CountryStats> CountryStats { get; set; }
        public virtual DbSet<TableDependancyTest> TableDependancyTest { get; set; }
        public virtual DbSet<vwScrapeRun> vwScrapeRuns { get; set; }
        
        public CvContext() { }

        public CvContext(DbContextOptions<CvContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(
                    "Data Source=TOM-HPENVY-16;Initial Catalog=CoronaVirus;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vwScrapeRun>().HasNoKey().ToView("vwScrapeRun");
        }

    }
}
