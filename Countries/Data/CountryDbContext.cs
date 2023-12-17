using Countries.Models;
using Microsoft.EntityFrameworkCore;

namespace Countries.Data
{
    public class CountryDbContext : DbContext
    {
        public CountryDbContext(DbContextOptions<CountryDbContext> options) : base(options) { }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.Name)
                .IsUnique(true);
            modelBuilder.Entity<Restaurant>()
                .HasIndex(r => r.Name)
                .IsUnique(true);
            modelBuilder.Entity<Hotel>()
                .HasIndex(h => h.Name)
                .IsUnique(true);

            modelBuilder.Entity<Country>()
                .HasMany(r => r.Restaurants)
                .WithMany(c => c.Countries)
                .UsingEntity<CountryRestaurant>();

            modelBuilder.Entity<Country>()
                .HasMany(h => h.Hotels)
                .WithMany(c => c.Countries)
                .UsingEntity<CountryHotel>();
        }
    }
}
