namespace EHBrewingMonitoringDashboard.Services;

using EHBrewingMonitoringDashboard.Data;
using EHBrewingMonitoringDashboard.DTOs;
using EHBrewingMonitoringDashboard.Enums;
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

    public async Task<List<FermenterStatusCountDto>> GetFermentersStatusCountAsync()
    {
        await using var context = await _factory.CreateDbContextAsync();

        var fermenters = await context.Fermenters.AsNoTracking()
            .Where(f => f.Active)
            .OrderBy(f => f.Name)
            .Select(f => new
            {
                f.Name,
                Sensors = f.Sensors
                    .Where(s => s.Active)
                    .Select(s => new
                    {
                        Sensor = s,
                        LastMeasure = s.Measures.OrderByDescending(m => m.RecordedAt).FirstOrDefault()
                    })
            })
            .ToListAsync();

        return [.. fermenters.Select(f =>
        {
            var counts = new Dictionary<EStatus, int>
            {
                [EStatus.Ok] = 0,
                [EStatus.Warning] = 0,
                [EStatus.Critical] = 0,
                [EStatus.NoData] = 0,
            };

            foreach (var s in f.Sensors)
            {
                var status = s.LastMeasure is null
                    ? EStatus.NoData
                    : _sensorService.GetStatusByValue(s.Sensor, s.LastMeasure.Value);
                counts[status]++;
            }

            return new FermenterStatusCountDto(
                f.Name,
                counts[EStatus.Ok],
                counts[EStatus.Warning],
                counts[EStatus.Critical],
                counts[EStatus.NoData]
            );
        })];
    }
}