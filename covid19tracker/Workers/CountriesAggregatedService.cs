using System;
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
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace covid19tracker.Workers
{
    public class CountriesAggregatedService : BackgroundService
    {
        private const string CUrl = "https://api.github.com/repos/datasets/covid-19/contents/data/countries-aggregated.csv";
        private const string CUserAgent = "covid19tracker app";

        private ILogger<CountriesAggregatedService> _logger;
        private readonly CountriesAggregatedServiceSettings _settings;
        private readonly IServiceProvider _services;

        private static string FeedId = DataFeedType.CountriesAggregated.ToString();
        private readonly Expression<Func<LastUpdate, bool>> LastUpdatePredicate = x => x.Id == FeedId;

        public CountriesAggregatedService(IOptions<CountriesAggregatedServiceSettings> settings, IServiceProvider services, ILogger<CountriesAggregatedService> logger)
        {
            _logger = logger;
            _settings = settings.Value;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(CountriesAggregatedService)} is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation($"{nameof(CountriesAggregatedService)} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Checking for new daily data.");

                using (IServiceScope scope = _services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<CountryAggregatedContext>();
                    var lastUpdateContext = scope.ServiceProvider.GetRequiredService<LastUpdateContext>();
                    await this.CheckForDailyData(db, lastUpdateContext);
                }

                _logger.LogDebug($"Waiting {_settings.CheckIntervalInHours} hours.");
                await Task.Delay(TimeSpan.FromHours(_settings.CheckIntervalInHours), stoppingToken);
            }

            _logger.LogInformation($"{nameof(CountriesAggregatedService)} is stopped.");
        }

        public async Task CheckForDailyData(CountryAggregatedContext db, LastUpdateContext lastUpdateDb)
        {
            var updateNeeded = await this.CheckIfUpdateNeeded(db, lastUpdateDb);
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
                        using (var streamReader = new StreamReader(stream)) await InsertNewRecords(db, streamReader);
                    }
                }

                // set last update to now
                var entity = await lastUpdateDb.LastUpdates.AsNoTracking().SingleAsync(this.LastUpdatePredicate);
                entity.Date = DateTime.UtcNow;
                lastUpdateDb.Update(entity);
                await lastUpdateDb.SaveChangesAsync();
            }
        }

        private async Task InsertNewRecords(CountryAggregatedContext db, StreamReader reader)
        {
            var addCnt = 0;
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = true;
                var records = csv.GetRecords<CountryAggregated>().ToList();
                foreach (var record in records)
                {
                    if (await db.CountriesData.SingleOrDefaultAsync(w => w.Date.Date == record.Date.Date && w.Country == record.Country) != null) continue;

                    // record missing -- needs to be inserted
                    db.CountriesData.Add(record);
                    addCnt++;
                }
                db.SaveChanges();
                _logger.LogInformation($"Added {addCnt} country data in the database");
            }
        }

        private async Task<bool> CheckIfUpdateNeeded(CountryAggregatedContext db, LastUpdateContext lastUpdateDb)
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

        private async Task<DateTime> GetLastUpdateAsync(LastUpdateContext db)
        {
            var lastUpdate = await db.LastUpdates.SingleOrDefaultAsync(this.LastUpdatePredicate);
            if (lastUpdate == null)
            {
                db.LastUpdates.Add(new LastUpdate { Id = FeedId, Date = DateTime.MinValue });
                await db.SaveChangesAsync();
                return DateTime.MinValue;
            }

            return lastUpdate.Date;
        }
    }
}
