
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace covid19tracker.Workers.RssNews
{
    public class RssNewsBackgroundServiceDeAt : RssNewsBackgroundService
    {
        public RssNewsBackgroundServiceDeAt(IOptions<RssNewsServiceSettings> settings,
            IServiceProvider services, ILogger<RssNewsBackgroundService> logger) : base(settings, services, logger, "AT", "de-AT")
        {
        }
    }
}
