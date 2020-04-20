
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using covid19tracker.Model;

namespace covid19tracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Covid19TrackerDatabase");

            services.AddDbContext<CountryContext>(opt => opt
            .UseSqlServer(connectionString, providerOptions => providerOptions.CommandTimeout(60))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddDbContext<WorldAggregatedContext>(opt => opt
            .UseSqlServer(connectionString, providerOptions => providerOptions.CommandTimeout(60))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
