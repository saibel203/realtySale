using Microsoft.EntityFrameworkCore;
using RealtySale.Shared.Data;

namespace RealtySale.Api.Models;

public class RealtySaleContext : DbContext
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Property> Properties { get; set; } = null!;
    public DbSet<PropertyType> PropertyTypes { get; set; } = null!;
    public DbSet<FurnishingType> FurnishingTypes { get; set; } = null!;
    public DbSet<UserProperty> FavouriteProperties { get; set; } = null!;
    
    public RealtySaleContext(DbContextOptions<RealtySaleContext> options)
        :base (options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProperty>()
            .HasKey(fk => new {fk.UserId, fk.PropertyId});

        modelBuilder.Entity<UserProperty>()
            .HasOne(uk => uk.User)
            .WithMany(fk => fk.UserProperties)
            .HasForeignKey(uk => uk.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserProperty>()
            .HasOne(uk => uk.Property)
            .WithMany(fk => fk.UserProperties)
            .HasForeignKey(uk => uk.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
