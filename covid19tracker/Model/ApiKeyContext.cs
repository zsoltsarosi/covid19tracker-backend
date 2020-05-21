using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;

namespace covid19tracker.Model
{
    public class ApiKeyContext : DbContext
    {
        public DbSet<ApiKey> ApiKeys { get; set; }

        public ApiKeyContext(DbContextOptions<ApiKeyContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(",", v), v => v.Split(new[] { ',' }));
            modelBuilder.Entity<ApiKey>().Property(nameof(ApiKey.Roles)).HasConversion(splitStringConverter);

            var initialApiKeys = this.GetDataSeed();
            modelBuilder.Entity<ApiKey>().HasData(initialApiKeys);
        }

        private IEnumerable<ApiKey> GetDataSeed()
        {
            var apiKeys = new List<ApiKey>()
            {
                new ApiKey() {
                    Key = Guid.NewGuid(),
                    Owner = "mobile app",
                    Created = DateTime.UtcNow,
                    Expiration = DateTime.UtcNow.Add(ApiKey.CDefaultExpiration),
                    Roles = new string[] { Roles.WorldData.ToString(), Roles.CountryData.ToString(), Roles.News.ToString() }
                }
            };
            return apiKeys;
        }
    }
}
