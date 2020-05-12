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
        private readonly CountryContext _countryDb;
        private readonly ILogger<CountryDataController> _logger;

        public CountryDataController(CountryAggregatedContext db, CountryContext countryDb, ILogger<CountryDataController> logger)
        {
            _db = db;
            _countryDb = countryDb;
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

            var lastDateEntries = await _db.CountriesData.Where(x => x.Date == lastDateEntry.Date).OrderBy(x => x.Country)
                .Select(x => new { t = x.Date.ToShortDateString(), n = x.Country, c = x.Confirmed, r = x.Recovered, d = x.Deaths, n2 = NameToIso(_countryDb, x.Country) })
                .ToListAsync();
            return lastDateEntries;
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
                .Select(x => new { t = x.Date.ToShortDateString(), c = x.Confirmed, r = x.Recovered, d = x.Deaths })
                .ToListAsync();
            return countryData;
        }

        private static string NameToIso(CountryContext countryDb, string name)
        {
            return countryDb.Countries.FirstOrDefault(c => c.Name == name)?.Iso2;
        }
    }
}
