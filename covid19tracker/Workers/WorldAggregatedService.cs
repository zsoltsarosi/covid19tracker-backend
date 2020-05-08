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
    public class WorldAggregatedService : GitHubFeedService<WorldAggregatedContext, WorldAggregatedService>
    {
        protected override string CUrl => "https://api.github.com/repos/datasets/covid-19/contents/data/worldwide-aggregated.csv";
        protected override string FeedId => DataFeedType.WorldAggregated.ToString();

        private readonly WorldAggregatedServiceSettings _settings;

        public WorldAggregatedService(IOptions<WorldAggregatedServiceSettings> settings, IServiceProvider services, ILogger<WorldAggregatedService> logger)
            : base(services, logger)
        {
            _settings = settings.Value;
            this.CCheckIntervalInHours = _settings.CheckIntervalInHours;
        }

        protected override async Task ParseAndInsertNewRecords(WorldAggregatedContext db, StreamReader reader)
        {
            var addCnt = 0;
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<WorldAggregatedMap>();
                var records = csv.GetRecords<WorldAggregated>().ToList();
                foreach (var record in records)
                {
                    if (await db.WorldData.SingleOrDefaultAsync(w => w.Date.Date == record.Date.Date) != null) continue;

                    // record missing -- needs to be inserted
                    db.WorldData.Add(record);
                    addCnt++;
                }
                db.SaveChanges();
                _logger.LogInformation($"Added {addCnt} world data in the database");
            }
        }

        protected override async Task<bool> CheckIfUpdateNeeded(WorldAggregatedContext db, LastUpdateContext lastUpdateDb)
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            if (await db.WorldData.SingleOrDefaultAsync(w => w.Date.Date == yesterday.Date) == null)
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
