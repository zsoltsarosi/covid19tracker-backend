using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using covid19tracker.Model;
using Microsoft.Extensions.Logging;
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using HtmlAgilityPack;
using System.Linq.Expressions;

namespace covid19tracker.DataFeed
{
    public class RssNewsFeed
    {
        private string locale = "en-US";
        private string country = "US";
        private static string Covid19TopicId = "CAAqIggKIhxDQkFTRHdvSkwyMHZNREZqY0hsNUVnSmxiaWdBUAE";
        private string _feedUrl = $"https://news.google.com/rss/topics/{Covid19TopicId}?hl={{0}}&gl={{1}}";

        private ILogger<RssNewsFeed> _logger;
        private RssNewsContext _dbContext;
        private LastUpdateContext _lastUpdateContext;

        private static string FeedId = DataFeedType.RssNews.ToString();
        private readonly Expression<Func<LastUpdate, bool>> LastUpdatePredicate = x => x.Id == FeedId;

        public RssNewsFeed(RssNewsContext dbContext, LastUpdateContext lastUpdateContext, ILogger<RssNewsFeed> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _lastUpdateContext = lastUpdateContext;

            FeedId = $"{DataFeedType.RssNews}#GoogleNews#{Covid19TopicId}#{country}#{locale}";
        }

        public async Task<IList<RssNews>> GetData()
        {
            var updateNeeded = await this.CheckIfUpdateNeeded();
            if (updateNeeded)
            {
                var feedUrl = string.Format(_feedUrl, locale, country);

                var feed = await FeedReader.ReadAsync(feedUrl);

                var addCnt = 0;
                foreach (var item in feed.Items)
                {
                    if (await _dbContext.News.SingleOrDefaultAsync(w => w.Id == item.Id) != null) continue;

                    // news missing -- needs to be inserted

                    var mrssItem = item.SpecificItem as MediaRssFeedItem;

                    _dbContext.News.Add(new RssNews
                    {
                        Id = item.Id,
                        FeedId = FeedId,
                        Date = item.PublishingDate.HasValue ? item.PublishingDate.Value.ToUniversalTime() : DateTime.UtcNow,
                        Title = item.Title,
                        Link = item.Link,
                        SourceName = mrssItem?.Source?.Value,
                        SourceUrl = mrssItem?.Source?.Url,
                    });
                    addCnt++;
                }

                _dbContext.SaveChanges();
                _logger.LogInformation($"Added {addCnt} news in the database");

                // set last update to now
                var entity = await _lastUpdateContext.LastUpdates.AsNoTracking().SingleAsync(this.LastUpdatePredicate);
                entity.Date = DateTime.UtcNow;
                _lastUpdateContext.Update(entity);
                await _lastUpdateContext.SaveChangesAsync();
            }

            // take the last 20 news
            var result = await _dbContext.News.OrderByDescending(x => x.Date).Take(20).ToListAsync();
            return result;
        }

        public async Task<byte[]> GetImageData(string newsId)
        {
            var newsItem = await _dbContext.News.SingleOrDefaultAsync(w => w.Id == newsId);
            if (newsItem == null) return new byte[0];

            byte[] imgContent = await DownloadImage(newsItem.Link);

            return imgContent;
        }

        private async Task<byte[]> DownloadImage(string link)
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
                            var imgUrl = metaImageNodes.FirstOrDefault()?.Attributes["content"]?.Value;
                            if (imgUrl != null)
                            {
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading image", link);
            }

            return imgContent;
        }

        private async Task<bool> CheckIfUpdateNeeded()
        {
            var lastUpdate = await this.GetLastUpdateAsync();
            if (DateTime.UtcNow - lastUpdate > TimeSpan.FromHours(1))
            {
                // we haven't checked for update in the past 1 hours
                return true;
            }

            return false;
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
