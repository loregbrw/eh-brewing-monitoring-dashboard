namespace EHBrewingMonitoringDashboard.Services;

using EHBrewingMonitoringDashboard.Data;
using EHBrewingMonitoringDashboard.DTOs;
using Microsoft.EntityFrameworkCore;

public class DashboardService(IDbContextFactory<AppDbContext> factory)
{
    private readonly IDbContextFactory<AppDbContext> _factory = factory;
    public async Task<DateTime?> GetLasAtt()
    {
        await using var context = await _factory.CreateDbContextAsync();

        return await context.Measures.AsNoTracking().MaxAsync(m => m.RecordedAt);
    }

    public async Task<List<FermenterOverviewDto>> GetFermentersOverviewAsync()
    {
         await using var context = await _factory.CreateDbContextAsync();

        var fermenters = await context.Fermenters.AsNoTracking()
            .Where(f => f.Active)
            .Select(f => new FermenterOverviewDto(
                f.Id,
                f.Name,
                f.Active,
                f.Sensors
                    .Where(s => s.Active)
                    .Select(s => new
                    {
                        s.Type,
                        s.MeasureUnit,
                        LastMeasure = s.Measures.OrderByDescending(m => m.RecordedAt).FirstOrDefault()
                    })
                    .Where(x => x.LastMeasure != null)
                    .GroupBy(x => x.Type)
                    .Select(g => new SensorReadingDto(
                        g.Key,
                        g.Average(x => x.LastMeasure!.Value),
                        g.First().MeasureUnit,
                        g.Max(x => x.LastMeasure!.RecordedAt)
                    ))
                    .ToList()
            ))
            .ToListAsync();

        return fermenters;
    }
}