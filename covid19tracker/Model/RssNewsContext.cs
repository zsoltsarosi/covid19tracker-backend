
using Microsoft.EntityFrameworkCore;

namespace covid19tracker.Model
{
    public class RssNewsContext : DbContext
    {
        public DbSet<RssNews> News { get; set; }

        public RssNewsContext(DbContextOptions<RssNewsContext> options) : base(options) { } 
    }
}
