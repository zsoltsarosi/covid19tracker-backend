
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace covid19tracker.Workers.RssNews
{
    public class RssNewsBackgroundServiceHuHu : RssNewsBackgroundService
    {
        public RssNewsBackgroundServiceHuHu(IOptions<RssNewsServiceSettings> settings,
            IServiceProvider services, ILogger<RssNewsBackgroundService> logger) : base(settings, services, logger, "HU", "hu-HU")
        {
        }
    }
}
