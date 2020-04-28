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
    public class WorldAggregatedController : ControllerBase
    {
        private readonly WorldAggregatedFeed _feed;
        private readonly ILogger<WorldAggregatedController> _logger;

        public WorldAggregatedController(WorldAggregatedFeed feed, ILogger<WorldAggregatedController> logger)
        {
            _feed = feed;
            _logger = logger;
        }

        // GET: api/worldaggregated
        [HttpGet]
        public async Task<ActionResult<IList<WorldAggregated>>> GetEntries()
        {
            var result = await _feed.GetData();
            return result.ToList();
        }
    }
}
