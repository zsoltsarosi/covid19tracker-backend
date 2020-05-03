using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using covid19tracker.Model;
using Microsoft.EntityFrameworkCore;

namespace covid19tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssNewsController : ControllerBase
    {
        private RssNewsContext _dbContext;
        private readonly ILogger<RssNewsController> _logger;

        public RssNewsController(RssNewsContext dbContext, ILogger<RssNewsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // GET: api/rssnews
        [HttpGet]
        public async Task<ActionResult<IList<RssNews>>> GetNews()
        {
            var result = await _dbContext.News.OrderByDescending(x => x.Date).Take(50).ToListAsync();
            return result;
        }

        // GET: api/rssnews/52780748426025
        [HttpGet("{newsId}")]
        public async Task<ActionResult<RssNews>> GetNews(string newsId)
        {
            var newsItem = await _dbContext.News.SingleOrDefaultAsync(w => w.Id == newsId);
            return newsItem;
        }
    }
}
