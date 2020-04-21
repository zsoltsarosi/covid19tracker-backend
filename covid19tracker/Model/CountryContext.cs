using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace covid19tracker.Model
{
    public class CountryContext : ContextBase<CountryContext>
    {
        public DbSet<Country> Countries { get; set; }

        public CountryContext(DbContextOptions<CountryContext> options) : base(options) { } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var countries = this.GetDataSeed();
            modelBuilder.Entity<Country>().HasData(countries);
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
