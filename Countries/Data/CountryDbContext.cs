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
            modelBuilder.Entity<CountryRestaurant>()
                .HasKey(cr => new {cr.CountryId, cr.RestaurantId});
            modelBuilder.Entity<CountryRestaurant>()
                .HasOne(cr => cr.Country)
                .WithMany(c => c.CountryRestaurants)
                .HasForeignKey(cr => cr.CountryId);
            modelBuilder.Entity<CountryRestaurant>()
                .HasOne(cr => cr.Restaurant)
                .WithMany(r => r.CountryRestaurants)
                .HasForeignKey(cr => cr.RestaurantId);

            modelBuilder.Entity<CountryHotel>()
                .HasKey(ch => new {ch.CountryId, ch.HotelId});
            modelBuilder.Entity<CountryHotel>()
                .HasOne(ch => ch.Country)
                .WithMany(c => c.CountryHotels)
                .HasForeignKey(ch => ch.CountryId);
            modelBuilder.Entity<CountryHotel>()
                .HasOne(ch => ch.Hotel)
                .WithMany(h => h.CountryHotels)
                .HasForeignKey(ch => ch.HotelId);

        }
    }
}
