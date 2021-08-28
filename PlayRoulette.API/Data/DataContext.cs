using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlayRoulette.API.Data.Entities;

namespace PlayRoulette.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Roulette> Roulettes { get; set; }

        public DbSet<HistoryRoulette> HistoryRoulettes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Roulette>().HasIndex(r => r.Name).IsUnique(); 
        }
    }
}
