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
    public class WorldAggregatedController : ControllerBase
    {
        private readonly WorldAggregatedContext _db;
        private readonly ILogger<WorldAggregatedController> _logger;

        public WorldAggregatedController(WorldAggregatedContext db, ILogger<WorldAggregatedController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: api/worldaggregated
        [HttpGet]
        public async Task<ActionResult<IList<WorldAggregated>>> GetEntries()
        {
            var result = await _db.WorldData.OrderBy(x => x.Date).ToListAsync();
            return result;
        }
    }
}
