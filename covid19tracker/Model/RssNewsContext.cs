
using Microsoft.EntityFrameworkCore;

namespace covid19tracker.Model
{
    public class RssNewsContext : ContextBase<RssNewsContext>
    {
        public DbSet<RssNews> News { get; set; }

        public RssNewsContext(DbContextOptions<RssNewsContext> options) : base(options) { } 
    }
}
