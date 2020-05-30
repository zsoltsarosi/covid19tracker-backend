using System;
using System.Linq;
using System.Threading.Tasks;
using covid19tracker.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.IO;
using CsvHelper;
using System.Globalization;
using Microsoft.Extensions.Options;

namespace covid19tracker.Workers
{
    public class CountriesAggregatedService : GitHubFeedService<CountryContext, CountriesAggregatedService>
    {
        protected override string CUrl => "https://api.github.com/repos/datasets/covid-19/contents/data/countries-aggregated.csv";
        protected override string FeedId => DataFeedType.CountriesAggregated.ToString();

        private readonly CountriesAggregatedServiceSettings _settings;

        public CountriesAggregatedService(IOptions<CountriesAggregatedServiceSettings> settings, IServiceProvider services, ILogger<CountriesAggregatedService> logger)
            : base(services, logger)
        {
            _settings = settings.Value;
            this.CCheckIntervalInHours = _settings.CheckIntervalInHours;
        }
        protected override async Task ParseAndInsertNewRecords(CountryContext db, StreamReader reader)
        {
            var addCnt = 0;
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                var records = csv.GetRecords<CountryAggregated>().ToList();
                foreach (var record in records)
                {
                    record.Country = record.Country.Replace("Taiwan*", "Taiwan");
                    if (await db.CountriesData.SingleOrDefaultAsync(w => w.Date.Date == record.Date.Date && w.Country == record.Country) != null) continue;

                    // record missing -- needs to be inserted
                    db.CountriesData.Add(record);
                    addCnt++;
                }
                db.SaveChanges();
                _logger.LogInformation($"Added {addCnt} country data in the database");
            }
        }

        protected override async Task<bool> CheckIfUpdateNeeded(CountryContext db, LastUpdateContext lastUpdateDb)
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            if (await db.CountriesData.FirstOrDefaultAsync(w => w.Date.Date == yesterday.Date) == null)
            {
                // if there's no data from yesterday
                var lastUpdate = await this.GetLastUpdateAsync(lastUpdateDb);
                // default to true since we have a check interval
                return true;
            }

            _logger.LogInformation("No need to update - data already exists from yesterday.");
            return false;
        }
    }
}
