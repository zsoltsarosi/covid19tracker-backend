using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace covid19tracker.Model
{
    public abstract class ContextBase<TContext> : DbContext where TContext : DbContext
    {
        public ContextBase(DbContextOptions<TContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("Covid19TrackerDatabase");
            options.UseSqlServer(connectionString);
        }
    }
}
