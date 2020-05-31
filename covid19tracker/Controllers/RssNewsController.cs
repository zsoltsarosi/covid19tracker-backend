using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using covid19tracker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using covid19tracker.Workers.RssNews;

namespace covid19tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RssNewsController : ControllerBase
    {
        private LocaleFallback _localeFallback;
        private RssNewsContext _dbContext;
        private readonly ILogger<RssNewsController> _logger;

        public RssNewsController(LocaleFallback localeFallback, RssNewsContext dbContext, ILogger<RssNewsController> logger)
        {
            _localeFallback = localeFallback;
            _dbContext = dbContext;
            _logger = logger;
        }

        // GET: api/rssnews
        [HttpGet]
        public async Task<ActionResult<IList<RssNews>>> GetNews(string language, string country)
        {
            var culture = _localeFallback.GetBestLocaleAndCountry(language, country);
            var result = await _dbContext.News
                .Where(x => x.FeedId.Contains($"#{culture.Item2}#{culture.Item1}"))
                .OrderByDescending(x => x.Date).Take(50).ToListAsync();
            return result;
        }

        // GET: api/rssnews/52780748426025
        [HttpGet("{newsId}")]
        public async Task<ActionResult<RssNews>> GetNews(string newsId)
        {
            var newsItem = await _dbContext.News.FirstOrDefaultAsync(w => w.Id == newsId);
            return newsItem;
        }
    }
}
