using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace covid19tracker.Model
{
    public class CountryContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }

        public CountryContext(DbContextOptions<CountryContext> options) : base(options) { } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var countries = this.GetDataSeed();
            modelBuilder.Entity<Country>().HasData(countries);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("Covid19TrackerDatabase");
            options.UseSqlServer(connectionString);
        }

        private IEnumerable<Country> GetDataSeed()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "DataSeed", "countries.csv");
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                var records = csv.GetRecords<Country>().ToList();
                return records;
            }
        }
    }
}
