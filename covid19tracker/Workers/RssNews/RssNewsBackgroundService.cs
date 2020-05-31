using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using covid19tracker.Model;
using HtmlAgilityPack;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace covid19tracker.Workers.RssNews
{
    public abstract class RssNewsBackgroundService : BackgroundService
    {
        private readonly ILogger<RssNewsBackgroundService> _logger;
        private readonly RssNewsServiceSettings _settings;
        private readonly IServiceProvider _services;

        private string _country;
        private string _locale;
        private static string Covid19TopicId = "CAAqIggKIhxDQkFTRHdvSkwyMHZNREZqY0hsNUVnSmxiaWdBUAE";
        private string _feedUrl = $"https://news.google.com/rss/topics/{Covid19TopicId}?hl={{0}}&gl={{1}}";

        private string FeedId = DataFeedType.RssNews.ToString();

        public RssNewsBackgroundService(IOptions<RssNewsServiceSettings> settings,
            IServiceProvider services, ILogger<RssNewsBackgroundService> logger,
            string country, string locale)
        {
            _logger = logger;
            _settings = settings.Value;
            _services = services;
            _country = country;
            _locale = locale;

            FeedId = $"{DataFeedType.RssNews}#GoogleNews#{Covid19TopicId}#{_country}#{_locale}";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(RssNewsBackgroundService)}#{_country}#{_locale} is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation($"{nameof(RssNewsBackgroundService)}#{_country}#{_locale} is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Checking for news.");

                using (IServiceScope scope = _services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<RssNewsContext>();
                    var lastUpdateContext = scope.ServiceProvider.GetRequiredService<LastUpdateContext>();
                    await this.CheckForNews(db, lastUpdateContext);

                    // delete old news
                    var deleted = await db.Database.ExecuteSqlRawAsync("DELETE FROM dbo.News WHERE Date < {0}", DateTime.UtcNow.AddDays(-_settings.RetentionInDays));
                    _logger.LogDebug($"Deleted {deleted} news older than {_settings.RetentionInDays} days.");
                }

                _logger.LogDebug($"Waiting {_settings.CheckIntervalInMinutes} minutes.");
                await Task.Delay(_settings.CheckIntervalInMinutes * 60 * 1000, stoppingToken);
            }

            _logger.LogInformation($"{nameof(RssNewsBackgroundService)}#{_country}#{_locale} is stopped.");
        }

        private async Task CheckForNews(RssNewsContext rssNewsContext, LastUpdateContext lastUpdateContext)
        {
            var updateNeeded = await this.CheckIfUpdateNeeded(lastUpdateContext);
            if (updateNeeded)
            {
                var feedUrl = string.Format(_feedUrl, _locale, _country);

                _logger.LogDebug($"Checking feed from: {feedUrl}");

                var feed = await FeedReader.ReadAsync(feedUrl);

                _logger.LogDebug($"Found {feed.Items.Count} items in feed.");

                var addCnt = 0;
                foreach (var item in feed.Items)
                {
                    // prevent duplicates if ID has changed
                    if (await rssNewsContext.News.FirstOrDefaultAsync(w => w.Id == item.Id || w.Link == item.Link) != null) continue;

                    // news missing -- needs to be inserted

                    _logger.LogDebug($"Parsing news {item.Title}::{item.PublishingDateString}");

                    if (item.PublishingDate.HasValue && item.PublishingDate < DateTime.UtcNow.AddDays(-_settings.RetentionInDays))
                    {
                        _logger.LogDebug("News too old, skipping.");
                        continue;
                    }

                    var mrssItem = item.SpecificItem as MediaRssFeedItem;

                    byte[] img = null;
                    var endUrl = await this.CheckForRedirectsAsync(item.Link);
                    if (endUrl != null)
                    {
                        img = await this.GetThumbnail(endUrl);
                    }

                    rssNewsContext.News.Add(new Model.RssNews
                    {
                        Id = item.Id,
                        FeedId = FeedId,
                        Date = item.PublishingDate.HasValue ? item.PublishingDate.Value.ToUniversalTime() : DateTime.UtcNow,
                        Title = item.Title,
                        Link = item.Link,
                        EndUrl = endUrl,
                        ImageData = img,
                        SourceName = mrssItem?.Source?.Value,
                        SourceUrl = mrssItem?.Source?.Url,
                    });
                    addCnt++;

                    // break after adding some news
                    if (addCnt == _settings.MaxNewsInsertAtOnce) break;
                }

                if (addCnt > 0) rssNewsContext.SaveChanges();
                _logger.LogInformation($"Added {addCnt} news in the database");

                // set last update to now
                var entity = await lastUpdateContext.LastUpdates.AsNoTracking().SingleAsync(x => x.Id == FeedId);
                entity.Date = DateTime.UtcNow;
                lastUpdateContext.Update(entity);
                await lastUpdateContext.SaveChangesAsync();
            }
        }

        private async Task<byte[]> GetThumbnail(string link)
        {
            byte[] imgContent = null;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(link))
                    {
                        response.EnsureSuccessStatusCode();

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            HtmlDocument doc = new HtmlDocument();
                            doc.Load(stream);
                            HtmlNodeCollection metaImageNodes = doc.DocumentNode.SelectNodes("/html/head/meta[@property='og:image']");
                            if (metaImageNodes == null)
                            {
                                metaImageNodes = doc.DocumentNode.SelectNodes("/html/head/meta[@property='og:image:secure_url']");
                            }
                            var imgUrl = metaImageNodes?.FirstOrDefault()?.Attributes["content"]?.Value;
                            if (imgUrl == null)
                            {
                                return null;
                            }

                            using (HttpClient client = new HttpClient())
                            {
                                using (var imgResponse = await httpClient.GetAsync(imgUrl))
                                {
                                    imgResponse.EnsureSuccessStatusCode();
                                    imgContent = await imgResponse.Content.ReadAsByteArrayAsync();
                                }
                            }
                        }
                    }
                }

                using (MagickImage image = new MagickImage(imgContent))
                {
                    var size = new MagickGeometry(300, 100);
                    size.IgnoreAspectRatio = false;
                    image.Resize(size);
                    image.Format = MagickFormat.Jpg;
                    imgContent = image.ToByteArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading image for: {link}");
            }

            return imgContent;
        }

        private async Task<string> CheckForRedirectsAsync(string url)
        {
            int redirectsLeft = 3;
            string endUrl = null;

            try
            {
                do
                {
                    var handler = new HttpClientHandler();
                    handler.AllowAutoRedirect = false; // do redirects manually
                    using (var httpClient = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(20) })
                    using (var response = await httpClient.GetAsync(url))
                    {
                        if (response.StatusCode == HttpStatusCode.MovedPermanently ||
                            response.StatusCode == HttpStatusCode.Moved ||
                            response.StatusCode == HttpStatusCode.Found)
                        {
                            url = response.Headers.Location.OriginalString;
                            redirectsLeft -= 1;
                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            url = response.RequestMessage.RequestUri.OriginalString;
                            redirectsLeft -= 1;
                        }
                        else
                        {
                            try
                            {
                                response.EnsureSuccessStatusCode();
                                endUrl = url;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Status code not success: {url}");
                                redirectsLeft = 0;
                            }
                        }
                    }
                } while (endUrl == null && redirectsLeft != 0);

                return endUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error extracting end url: {url}");
                return null;
            }
        }

        private async Task<bool> CheckIfUpdateNeeded(LastUpdateContext dbContext)
        {
            var lastUpdate = await this.GetLastUpdateAsync(dbContext);
            // default to true since we have a check interval
            return true;
        }

        private async Task<DateTime> GetLastUpdateAsync(LastUpdateContext db)
        {
            var lastUpdate = await db.LastUpdates.SingleOrDefaultAsync(x => x.Id == FeedId);
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
