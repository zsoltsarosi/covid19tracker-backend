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

namespace covid19tracker.DataFeed
{
    public class WorldAggregatedFeed
    {
        private const string CUrl = "https://api.github.com/repos/datasets/covid-19/contents/data/worldwide-aggregated.csv";
        private const string CUserAgent = "covid19tracker app";

        private ILogger<WorldAggregatedFeed> _logger;
        private WorldAggregatedContext _dbContext;
        private LastUpdateContext _lastUpdateContext;

        public WorldAggregatedFeed(WorldAggregatedContext dbContext, LastUpdateContext lastUpdateContext, ILogger<WorldAggregatedFeed> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _lastUpdateContext = lastUpdateContext;
        }

        public async Task<IList<WorldAggregated>> GetData()
        {
            var updateNeeded = this.CheckIfUpdateNeeded();
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
                        using (var streamReader = new StreamReader(stream)) InsertNewRecords(streamReader);
                    }
                }

                _lastUpdateContext.LastUpdates.First(m => m.DataFeed == DataFeedType.WorldAggregated).Date = DateTime.UtcNow;
                await _lastUpdateContext.SaveChangesAsync();
            }
            var result = await _dbContext.WorldData.ToListAsync();
            return result;
        }

        private void InsertNewRecords(StreamReader streamReader)
        {
            using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<WorldAggregatedMap>();
                var records = csv.GetRecords<WorldAggregated>().ToList();
                foreach (var record in records)
                {
                    if (_dbContext.WorldData.FirstOrDefault(w => w.Date.Date == record.Date.Date) == null)
                    {
                        // record missing -- needs to be inserted
                        _dbContext.WorldData.Add(record);
                    }
                    //  (no updates)
                }
                _dbContext.SaveChanges();
            }
        }

        private bool CheckIfUpdateNeeded()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            if (_dbContext.WorldData.FirstOrDefault(w => w.Date.Date == yesterday.Date) == null)
            {
                // if there's data from yesterday
                var lastUpdate = this.GetLastUpdate();
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

        private DateTime GetLastUpdate()
        {
            var lastUpdate = _lastUpdateContext.LastUpdates.FirstOrDefault(u => u.DataFeed == DataFeedType.WorldAggregated);
            if (lastUpdate == null) return DateTime.MinValue;

            return lastUpdate.Date;
        }
    }
}
