using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace covid19tracker.Model
{
    public class WorldAggregatedContext : ContextBase<WorldAggregatedContext>
    {
        public DbSet<WorldAggregated> WorldData { get; set; }

        public WorldAggregatedContext(DbContextOptions<WorldAggregatedContext> options) : base(options) { } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var worldData = this.GetDataSeed();
            modelBuilder.Entity<WorldAggregated>().HasData(worldData);
        }

        private IEnumerable<WorldAggregated> GetDataSeed()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "DataSeed", "worldwide-aggregated.csv");
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<WorldAggregatedMap>();
                var records = csv.GetRecords<WorldAggregated>().ToList();
                return records;
            }
        }
    }

    public class WorldAggregatedMap : ClassMap<WorldAggregated>
    {
        public WorldAggregatedMap()
        {
            Map(m => m.Date);
            Map(m => m.Confirmed);
            Map(m => m.Recovered);
            Map(m => m.Deaths);
            Map(m => m.IncreaseRate).Name("Increase rate").TypeConverterOption.Format("0.0000");
        }
    }
}
