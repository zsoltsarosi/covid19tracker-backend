using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using covid19tracker.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Linq.Expressions;

namespace covid19tracker.DataFeed
{
    public class WorldAggregatedFeed
    {
        private const string CUrl = "https://api.github.com/repos/datasets/covid-19/contents/data/worldwide-aggregated.csv";
        private const string CUserAgent = "covid19tracker app";

        private ILogger<WorldAggregatedFeed> _logger;
        private WorldAggregatedContext _dbContext;
        private LastUpdateContext _lastUpdateContext;

        private static string FeedId = DataFeedType.WorldAggregated.ToString();
        private readonly Expression<Func<LastUpdate, bool>> LastUpdatePredicate = x => x.Id == FeedId;

        public WorldAggregatedFeed(WorldAggregatedContext dbContext, LastUpdateContext lastUpdateContext, ILogger<WorldAggregatedFeed> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _lastUpdateContext = lastUpdateContext;
        }

        public async Task<IList<WorldAggregated>> GetData()
        {
            var updateNeeded = await this.CheckIfUpdateNeeded();
            if (updateNeeded)
            {
                var downloadUrl = await this.GetDownloadUrl();
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(CUserAgent);

                    using (var response = await httpClient.GetAsync(downloadUrl))
                    {
                        response.EnsureSuccessStatusCode();

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var streamReader = new StreamReader(stream)) await InsertNewRecords(streamReader);
                    }
                }

                // set last update to now
                var entity = await _lastUpdateContext.LastUpdates.AsNoTracking().SingleAsync(this.LastUpdatePredicate);
                entity.Date = DateTime.UtcNow;
                _lastUpdateContext.Update(entity);
                await _lastUpdateContext.SaveChangesAsync();
            }
            var result = await _dbContext.WorldData.OrderBy(x => x.Date).ToListAsync();
            return result;
        }

        private async Task InsertNewRecords(StreamReader streamReader)
        {
            var addCnt = 0;
            using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<WorldAggregatedMap>();
                var records = csv.GetRecords<WorldAggregated>().ToList();
                foreach (var record in records)
                {
                    if (await _dbContext.WorldData.SingleOrDefaultAsync(w => w.Date.Date == record.Date.Date) != null) continue;

                    // record missing -- needs to be inserted
                    _dbContext.WorldData.Add(record);
                    addCnt++;
                }
                _dbContext.SaveChanges();
                _logger.LogInformation($"Added {addCnt} world data in the database");
            }
        }

        private async Task<bool> CheckIfUpdateNeeded()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            if (await _dbContext.WorldData.SingleOrDefaultAsync(w => w.Date.Date == yesterday.Date) == null)
            {
                // if there's data from yesterday
                var lastUpdate = await this.GetLastUpdateAsync();
                if (DateTime.UtcNow - lastUpdate > TimeSpan.FromHours(2))
                {
                    // and haven't checked for update in the past 2 hours
                    return true;
                }
            }

            return false;
        }

        private async Task<string> GetDownloadUrl()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(CUserAgent);

                using (var response = await httpClient.GetAsync(CUrl))
                {
                    response.EnsureSuccessStatusCode();

                    using (var content = response.Content)
                    {
                        //get the json result from your api
                        var result = await content.ReadAsStringAsync();

                        using (JsonDocument document = JsonDocument.Parse(result))
                        {
                            JsonElement root = document.RootElement;
                            var downloadUrl = root.GetProperty("download_url").GetString();
                            return downloadUrl;
                        }
                    }
                }
            }
        }

        private async Task<DateTime> GetLastUpdateAsync()
        {
            var lastUpdate = await _lastUpdateContext.LastUpdates.SingleOrDefaultAsync(this.LastUpdatePredicate);
            if (lastUpdate == null)
            {
                _lastUpdateContext.LastUpdates.Add(new LastUpdate { Id = FeedId, Date = DateTime.MinValue });
                await _lastUpdateContext.SaveChangesAsync();
                return DateTime.MinValue;
            }

            return lastUpdate.Date;
        }
    }
}
