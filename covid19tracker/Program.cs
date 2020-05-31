using covid19tracker.Workers;
using covid19tracker.Workers.RssNews;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace covid19tracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<WorldAggregatedService>();
                    services.AddHostedService<CountriesAggregatedService>();

                    services.AddHostedService<RssNewsBackgroundServiceHuHu>();
                    services.AddHostedService<RssNewsBackgroundServiceJpJp>();
                    services.AddHostedService<RssNewsBackgroundServiceNlBe>();
                    services.AddHostedService<RssNewsBackgroundServiceFrBe>();

                    services.AddHostedService<RssNewsBackgroundServiceEnGb>();
                    services.AddHostedService<RssNewsBackgroundServiceEnUs>();

                    services.AddHostedService<RssNewsBackgroundServiceDeAt>();
                    services.AddHostedService<RssNewsBackgroundServiceDeDe>();
                    services.AddHostedService<RssNewsBackgroundServiceDeCh>();
                })
            ;
    }
}
