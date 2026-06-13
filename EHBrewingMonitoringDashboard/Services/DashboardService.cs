namespace EHBrewingMonitoringDashboard.Services;

using EHBrewingMonitoringDashboard.Data;
using EHBrewingMonitoringDashboard.DTOs;
using Microsoft.EntityFrameworkCore;

public class DashboardService(IDbContextFactory<AppDbContext> factory, SensorService sensorService)
{
    private readonly IDbContextFactory<AppDbContext> _factory = factory;
    private readonly SensorService _sensorService = sensorService;

    public async Task<DateTime?> GetLasAtt()
    {
        await using var context = await _factory.CreateDbContextAsync();
        return await context.Measures.AsNoTracking().MaxAsync(m => (DateTime?)m.RecordedAt);
    }

    public async Task<List<FermenterOverviewDto>> GetFermentersOverviewAsync()
    {
        await using var context = await _factory.CreateDbContextAsync();

        var fermenters = await context.Fermenters
            .AsNoTracking()
            .Where(f => f.Active)
            .OrderBy(f => f.Name)
            .Select(f => new
            {
                f.Id,
                f.Name,
                f.Active,
                Sensors = f.Sensors
                    .Where(s => s.Active)
                    .Select(s => new
                    {
                        Sensor = s,
                        LastMeasure = s.Measures
                            .OrderByDescending(m => m.RecordedAt)
                            .FirstOrDefault()
                    })
                    .Where(x => x.LastMeasure != null)
                    .ToList()
            })
            .ToListAsync();

        return [.. fermenters
                .Select(f => new FermenterOverviewDto(
                    f.Id,
                    f.Name,
                    f.Active,
                    [.. f.Sensors
                        .GroupBy(x => x.Sensor.Type)
                        .Select(g =>
                        {
                            var status = g.Max(x => _sensorService.GetStatusByValue(
                                x.Sensor,
                                x.LastMeasure!.Value));

                            return new SensorReadingDto(
                                g.Key,
                                g.Average(x => x.LastMeasure!.Value),
                                g.First().Sensor.MeasureUnit,
                                g.Max(x => x.LastMeasure!.RecordedAt),
                                status
                            );
                        })]
                ))];
    }

    public async Task<List<SensorMaintenanceDto>> GetTopSensorsMaintenanceAsync()
    {
        await using var context = await _factory.CreateDbContextAsync();

        return await context.Sensors
            .AsNoTracking()
            .Where(s => s.Active)
            .Select(s => new
            {
                Name = $"{s.SerialNumber} ({s.Fermenter.Name}/{s.Type})",
                Date = s.LastMaintenanceAt ?? s.InstalledAt
            })
            .ToListAsync()
            .ContinueWith(t => t.Result
                .Select(x => new SensorMaintenanceDto(
                    x.Name,
                    (DateTime.UtcNow - x.Date).Days
                ))
                .OrderByDescending(x => x.Days)
                .Take(5)
                .ToList());
    }
}