using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class AppAlionaContext : DbContext
    {
        public AppAlionaContext(DbContextOptions<AppAlionaContext> options)
            : base(options) { }

        public DbSet<Banan> Banans { get; set; }
        public DbSet<News> News { get; set; }
    }
}
