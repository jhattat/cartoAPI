using Microsoft.EntityFrameworkCore;

namespace ObsApi.Models
{
    public class AzureDbContext : DbContext
    {
        public AzureDbContext(DbContextOptions<AzureDbContext> options)
               : base(options)
        {
        }

        public DbSet<AZObsItem> AZObsItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AZObsItem>().Ignore(x => x.Geo);

        }
    }
}