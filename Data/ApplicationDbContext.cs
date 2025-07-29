using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MP_Backend.Models;

namespace MP_Backend.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>()
                .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.UserProfile)
                .WithMany()
                .HasForeignKey(o => o.UserProfileId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.UserProfile)
                .WithMany()
                .HasForeignKey(b => b.UserProfileId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
