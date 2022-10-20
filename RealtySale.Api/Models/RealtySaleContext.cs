﻿using Microsoft.EntityFrameworkCore;

namespace RealtySale.Api.Models;

public class RealtySaleContext : DbContext
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    
    public RealtySaleContext(DbContextOptions<RealtySaleContext> options)
        :base (options)
    {
        
    }
}