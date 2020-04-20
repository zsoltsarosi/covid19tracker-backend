using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using covid19tracker.Model;

namespace covid19tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryContext _context;
        private readonly ILogger<CountryController> _logger;

        public CountryController(CountryContext context, ILogger<CountryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Country
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(string id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Country/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        internal async Task<IActionResult> PutCountry(string id, Country country)
        {
            if (id != country.Iso2)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Country
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        internal async Task<ActionResult<Country>> PostCountry(Country country)
        {
            _context.Countries.Add(country);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CountryExists(country.Iso2))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCountry", new { id = country.Iso2 }, country);
        }

        // DELETE: api/Country/5
        [HttpDelete("{id}")]
        internal async Task<ActionResult<Country>> DeleteCountry(string id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return country;
        }

        private bool CountryExists(string id)
        {
            return _context.Countries.Any(e => e.Iso2 == id);
        }
    }
}
