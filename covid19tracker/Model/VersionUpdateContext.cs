using Microsoft.EntityFrameworkCore;

namespace covid19tracker.Model
{
    public class VersionUpdateContext : DbContext
    {
        public DbSet<VersionUpdate> VersionUpdate { get; set; }

        public VersionUpdateContext(DbContextOptions<VersionUpdateContext> options) : base(options) { }
    }
}
