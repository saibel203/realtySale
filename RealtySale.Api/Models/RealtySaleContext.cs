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
    
    public RealtySaleContext(DbContextOptions<RealtySaleContext> options)
        :base (options)
    {
        
    }
}
