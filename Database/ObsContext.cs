using Microsoft.EntityFrameworkCore;

namespace ObsApi.Models
{
    public class ObsContext : DbContext
    {
        public ObsContext(DbContextOptions<ObsContext> options)
            : base(options)
        {
        }

        public DbSet<ObsItem> ObsItems { get; set; }

    }
}