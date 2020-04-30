using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using covid19tracker.Model;
using covid19tracker.DataFeed;

namespace covid19tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssNewsController : ControllerBase
    {
        private readonly RssNewsFeed _feed;
        private readonly ILogger<RssNewsController> _logger;

        public RssNewsController(RssNewsFeed feed, ILogger<RssNewsController> logger)
        {
            _feed = feed;
            _logger = logger;
        }

        // GET: api/rssnews
        [HttpGet]
        public async Task<ActionResult<IList<RssNews>>> GetNews()
        {
            var result = await _feed.GetData();
            return result.ToList();
        }

        // GET: api/rssnews/52780748426025
        [HttpGet("{newsId}")]
        public async Task<ActionResult<RssNews>> GetNews(string newsId)
        {
            var result = await _feed.GetNews(newsId);
            return result;
        }
    }
}
