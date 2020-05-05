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
    public class CountryDataController : ControllerBase
    {
        private readonly CountryAggregatedContext _db;
        private readonly ILogger<CountryDataController> _logger;

        public CountryDataController(CountryAggregatedContext db, ILogger<CountryDataController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: api/countrydata
        [HttpGet]
        public async Task<ActionResult<IList<CountryAggregated>>> GetEntries()
        {
            var lastDateEntry = await _db.CountriesData.OrderByDescending(x => x.Date).FirstOrDefaultAsync();
            if (lastDateEntry == null) // no entires in db
            {
                return Enumerable.Empty<CountryAggregated>().ToList();
            }

            var lastDateEntries = await _db.CountriesData.Where(x => x.Date == lastDateEntry.Date).OrderBy(x => x.Country).ToListAsync();
            return lastDateEntries;
        }
    }
}
