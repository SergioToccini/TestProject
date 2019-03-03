using Microsoft.EntityFrameworkCore;
using TestProject.Domain.Entities;
using TestProject.Entities;

namespace TestProject.Infrastructure.Database
{
    public sealed class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Store>().HasOne(x => x.StoreManager).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Store>().HasOne(x => x.Country).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Store>().HasOne(x => x.Stock).WithOne().OnDelete(DeleteBehavior.Cascade);

        }

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }


        public DbSet<Logs> Logs { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<StoreManager> StoreManagers { get; set; }

    }
}
