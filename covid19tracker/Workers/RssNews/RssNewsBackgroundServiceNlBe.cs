
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace covid19tracker.Workers.RssNews
{
    public class RssNewsBackgroundServiceNlBe : RssNewsBackgroundService
    {
        public RssNewsBackgroundServiceNlBe(IOptions<RssNewsServiceSettings> settings,
            IServiceProvider services, ILogger<RssNewsBackgroundService> logger) : base(settings, services, logger, "BE", "nl-BE")
        {
        }
    }
}
