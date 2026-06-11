namespace EHBrewingMonitoringDashboard.Data;

using EHBrewingMonitoringDashboard.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Fermenter> Fermenters => Set<Fermenter>();

    public DbSet<Sensor> Sensors => Set<Sensor>();

    public DbSet<Measure> Measures => Set<Measure>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}