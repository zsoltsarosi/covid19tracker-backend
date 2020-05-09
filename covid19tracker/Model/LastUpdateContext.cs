using Microsoft.EntityFrameworkCore;

namespace covid19tracker.Model
{
    public class LastUpdateContext : DbContext
    {
        public DbSet<LastUpdate> LastUpdates { get; set; }

        public LastUpdateContext(DbContextOptions<LastUpdateContext> options) : base(options) { }
    }
}
