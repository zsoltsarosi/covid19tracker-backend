using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace covid19tracker.Model
{
    public class CountryAggregatedContext : DbContext
    {
        public DbSet<CountryAggregated> CountriesData { get; set; }

        public CountryAggregatedContext(DbContextOptions<CountryAggregatedContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CountryAggregated>()
                .HasKey(c => new { c.Date, c.Country });

            var countriesData = this.GetDataSeed();
            modelBuilder.Entity<CountryAggregated>().HasData(countriesData);
        }

        private IEnumerable<CountryAggregated> GetDataSeed()
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
