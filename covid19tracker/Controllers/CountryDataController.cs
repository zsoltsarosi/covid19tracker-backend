using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using covid19tracker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace covid19tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountryDataController : ControllerBase
    {
        private readonly CountryContext _db;
        private readonly ILogger<CountryDataController> _logger;

        public CountryDataController(CountryContext db, ILogger<CountryDataController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: api/countrydata
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEntries()
        {
            if (!_db.CountriesData.Any())
            {
                // no entries in db
                return Enumerable.Empty<CountryAggregated>().ToList();
            }
            var lastDateEntry = await _db.CountriesData.OrderByDescending(x => x.Date).FirstOrDefaultAsync();
            if (lastDateEntry == null) // no entries in db
            {
                // that's weird
                _logger.LogWarning("No reasonable (?) data in CountriesData");
                return Enumerable.Empty<CountryAggregated>().ToList();
            }

            var lastEntries = from cd in _db.CountriesData
                              join c in _db.Countries
                                on cd.Country equals c.Name
                              where cd.Date == lastDateEntry.Date
                              select new { t = cd.Date, n = cd.Country, c = cd.Confirmed, r = cd.Recovered, d = cd.Deaths, n2 = c.Iso2, lat = c.Lat, lon = c.Long };

            return await lastEntries.ToListAsync();
        }

        // GET: api/countrydata/{country}
        [HttpGet("{country}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCountryDetails(string country)
        {
            if (!_db.CountriesData.Any())
            {
                // no entries in db
                return Enumerable.Empty<CountryAggregated>().ToList();
            }

            var countryData = await _db.CountriesData.Where(x => x.Country == country).OrderBy(x => x.Date)
                .Select(x => new { t = x.Date, c = x.Confirmed, r = x.Recovered, d = x.Deaths })
                .ToListAsync();
            return countryData;
        }
    }
}
