using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;

namespace covid19tracker.Model
{
    public class LastUpdateContext : ContextBase<LastUpdateContext>
    {
        public DbSet<LastUpdate> LastUpdates { get; set; }

        public LastUpdateContext(DbContextOptions<LastUpdateContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var converter = new EnumToStringConverter<DataFeedType>();

            modelBuilder
                .Entity<LastUpdate>()
                .Property(e => e.DataFeed)
                .HasConversion(converter);

            var initialLastUpdates = new List<LastUpdate>
            {
                new LastUpdate { DataFeed = DataFeedType.WorldAggregated, Date = DateTime.MinValue },
            };
            modelBuilder.Entity<LastUpdate>().HasData(initialLastUpdates);
        }
    }
}
