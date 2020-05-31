
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace covid19tracker.Workers.RssNews
{
    public class RssNewsBackgroundServiceFrBe : RssNewsBackgroundService
    {
        public RssNewsBackgroundServiceFrBe(IOptions<RssNewsServiceSettings> settings,
            IServiceProvider services, ILogger<RssNewsBackgroundService> logger) : base(settings, services, logger, "BE", "fr-BE")
        {
        }
    }
}
