
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace covid19tracker.Workers.RssNews
{
    public class RssNewsBackgroundServiceEnGb : RssNewsBackgroundService
    {
        public RssNewsBackgroundServiceEnGb(IOptions<RssNewsServiceSettings> settings,
            IServiceProvider services, ILogger<RssNewsBackgroundService> logger) : base(settings, services, logger, "UK", "en-GB")
        {
        }
    }
}
