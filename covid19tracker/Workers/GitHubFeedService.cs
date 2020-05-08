using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using covid19tracker.Model;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq.Expressions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace covid19tracker.Workers
{
    public abstract class GitHubFeedService<DbContextType, U> : BackgroundService
        where DbContextType : DbContext
        where U : GitHubFeedService<DbContextType, U>
    {
        protected virtual string CUrl => "";
        protected string CUserAgent => "covid19tracker app";

        protected int CCheckIntervalInHours { get; set; } = 12;

        protected ILogger<U> _logger;
        private readonly IServiceProvider _services;

        protected virtual string FeedId => "";
        private Expression<Func<LastUpdate, bool>> LastUpdatePredicate;

        public GitHubFeedService(IServiceProvider services, ILogger<U> logger)
        {
            _logger = logger;
            _services = services;
            LastUpdatePredicate = x => x.Id == FeedId;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{this.GetType().Name} is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation($"{this.GetType().Name} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Checking for new daily data.");

                using (IServiceScope scope = _services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DbContextType>();
                    var lastUpdateContext = scope.ServiceProvider.GetRequiredService<LastUpdateContext>();
                    await this.CheckForDailyData(dbContext, lastUpdateContext);
                }

                _logger.LogDebug($"Waiting {CCheckIntervalInHours} hours.");
                await Task.Delay(TimeSpan.FromHours(CCheckIntervalInHours), stoppingToken);
            }

            _logger.LogInformation($"{this.GetType().Name} is stopped.");
        }

        public async Task CheckForDailyData(DbContextType db, LastUpdateContext lastUpdateDb)
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
                        using (var streamReader = new StreamReader(stream)) await ParseAndInsertNewRecords(db, streamReader);
                    }
                }

                // set last update to now
                var entity = await lastUpdateDb.LastUpdates.AsNoTracking().SingleAsync(this.LastUpdatePredicate);
                entity.Date = DateTime.UtcNow;
                lastUpdateDb.Update(entity);
                await lastUpdateDb.SaveChangesAsync();
            }
        }

        protected abstract Task ParseAndInsertNewRecords(DbContextType db, StreamReader reader);

        protected abstract Task<bool> CheckIfUpdateNeeded(DbContextType db, LastUpdateContext lastUpdateDb);

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

        protected async Task<DateTime> GetLastUpdateAsync(LastUpdateContext db)
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
