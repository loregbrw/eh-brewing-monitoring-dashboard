namespace EHBrewingMonitoringDashboard.Services;

using EHBrewingMonitoringDashboard.Data;
using EHBrewingMonitoringDashboard.DTOs;
using Microsoft.EntityFrameworkCore;

public class DashboardService(AppDbContext db)
{
    public async Task<List<FermenterOverviewDto>> GetFermentersOverviewAsync()
    {
        var fermenters = await db.Fermenters.AsNoTracking()
            .Select(f => new FermenterOverviewDto
            (
                f.Id,
                f.Name,
                f.Active,
                f.Sensors.Select(s => new
                {
                    Sensor = s,
                    LastMeasure = s.Measures.OrderByDescending(m => m.RecordedAt).FirstOrDefault()
                })
                .Select(sm => new SensorReadingDto
                (
                    sm.Sensor.Id,
                    sm.Sensor.SerialNumber,
                    sm.Sensor.Active,
                    sm.LastMeasure != null ? sm.LastMeasure.Value : null,
                    sm.Sensor.MeasureUnit,
                    sm.LastMeasure != null ? sm.LastMeasure.RecordedAt : null
                )).ToList()
            ))
            .ToListAsync();

        return fermenters;
    }
}