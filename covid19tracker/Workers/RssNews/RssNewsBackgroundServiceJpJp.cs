
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace covid19tracker.Workers.RssNews
{
    public class RssNewsBackgroundServiceJpJp : RssNewsBackgroundService
    {
        public RssNewsBackgroundServiceJpJp(IOptions<RssNewsServiceSettings> settings,
            IServiceProvider services, ILogger<RssNewsBackgroundService> logger) : base(settings, services, logger, "JP", "jp-JP")
        {
        }
    }
}
