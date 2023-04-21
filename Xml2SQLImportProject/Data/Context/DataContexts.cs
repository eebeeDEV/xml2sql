
using Microsoft.EntityFrameworkCore;
using Xml2SqlImport.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xml2SqlImport.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xml2SqlImport.Data.SPModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Xml2SqlImport.Data.Context
{
    public class LocalDbContext : DbContext
    {
        private readonly IConfiguration _config;


        public LocalDbContext(DbContextOptions<LocalDbContext> options, IConfiguration config) : base(options)
        {
            _config = config;

        }

        // declare the DBSets
        // go to https://codverter.com/src/sqltoclass to convert SQL class to C# class
        public DbSet<FileCatalogRec>? FilesCatalog { get; set; }
        public DbSet<spIMPORT_SyncXmlModelRec>? TableModel { get; set; }
        public DbSet<spIMPORT_GetNextFileInfo>? FileInfo { get; set; }
        public DbSet<spIMPORT_GetLastFileImportDates>? LastImportFiles { get; set; }
        public DbSet<spIMPORT_GetLastKpiDateValues>? LastKpiDateValues { get; set; }
        public DbSet<spIMPORT_GetLast2KpiDateValues>? Last2KpiDateValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //var schema = _settings.GetValue<string>("localSchema");
            // add the _db entities or the data models returned by a stored procedure (with .hasNoKey())
            //modelBuilder.HasDefaultSchema(schema);
            modelBuilder.Entity<FileCatalogRec>();

            // stored procedure return record models
            modelBuilder.Entity<spGetTableFieldsRec>().HasNoKey();
            modelBuilder.Entity<spIMPORT_SyncXmlModelRec>().HasNoKey();
            modelBuilder.Entity<spIMPORT_GetNextFileInfo>().HasNoKey();
            modelBuilder.Entity<spIMPORT_GetLastFileImportDates>().HasNoKey();
            modelBuilder.Entity<spIMPORT_GetLastKpiDateValues>().HasNoKey();
            modelBuilder.Entity<spIMPORT_GetLast2KpiDateValues>().HasNoKey();


        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        ////=> optionsBuilder.ConfigureWarnings(warnings =>
        ////    warnings.Default(WarningBehavior.Ignore)).LogTo();
        //{
        //    if (optionsBuilder!.IsConfigured == false)
        //    {
        //        Console.WriteLine("ok");
                
        //        //if (optionsBuilder.EnableDetailedErrors)
        //        //{
        //        //    //optionsBuilder.UseLoggerFactory(LoggerFactory);
        //        //    // ...
        //        //}
        //    }
        //}

    }

    public class LogDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public LogDbContext(DbContextOptions<LogDbContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        // declare the DBSets
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //var schema = _settings.GetValue<string>("logSchema");
            //// add the _db entities or the data models returned by a stored procedure (with .hasNoKey())
            //modelBuilder.HasDefaultSchema(schema);

        }
    }

}
