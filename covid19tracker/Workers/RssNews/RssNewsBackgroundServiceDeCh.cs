
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace covid19tracker.Workers.RssNews
{
    public class RssNewsBackgroundServiceDeCh : RssNewsBackgroundService
    {
        public RssNewsBackgroundServiceDeCh(IOptions<RssNewsServiceSettings> settings,
            IServiceProvider services, ILogger<RssNewsBackgroundService> logger) : base(settings, services, logger, "CH", "de-CH")
        {
        }
    }
}
