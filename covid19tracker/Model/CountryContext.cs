using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace covid19tracker.Model
{
    public class CountryContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryAggregated> CountriesData { get; set; }

        public CountryContext(DbContextOptions<CountryContext> options) : base(options) { } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var countries = this.GetCountryDataSeed();
            modelBuilder.Entity<Country>().HasData(countries);

            modelBuilder.Entity<CountryAggregated>()
                .HasKey(c => new { c.Date, c.Country });

            var countriesData = this.GetCountryAggregatedDataSeed();
            modelBuilder.Entity<CountryAggregated>().HasData(countriesData);
        }

        private IEnumerable<Country> GetCountryDataSeed()
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

        private IEnumerable<CountryAggregated> GetCountryAggregatedDataSeed()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "DataSeed", "countries-aggregated.csv");
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                var records = csv.GetRecords<CountryAggregated>().ToList();
                return records;
            }
        }
    }
}
