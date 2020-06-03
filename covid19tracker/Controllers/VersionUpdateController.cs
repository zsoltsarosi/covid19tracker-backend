using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using covid19tracker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;

namespace covid19tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VersionUpdateController : ControllerBase
    {
        private readonly VersionUpdateContext _dbContext;
        private readonly ILogger<VersionUpdateController> _logger;

        public VersionUpdateController(VersionUpdateContext dbContext, ILogger<VersionUpdateController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // GET: api/versionupdate
        [HttpGet]
        public async Task<ActionResult<DateTime>> GetExpiration(string version)
        {
            var result = await _dbContext.VersionUpdate
                .Where(x => x.Version == version)
                .FirstOrDefaultAsync();
            return result?.Expiration;
        }
    }
}
