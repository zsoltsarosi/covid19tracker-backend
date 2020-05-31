
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace covid19tracker.Workers.RssNews
{
    public class RssNewsBackgroundServiceDeDe : RssNewsBackgroundService
    {
        public RssNewsBackgroundServiceDeDe(IOptions<RssNewsServiceSettings> settings,
            IServiceProvider services, ILogger<RssNewsBackgroundService> logger) : base(settings, services, logger, "DE", "de-DE")
        {
        }
    }
}
