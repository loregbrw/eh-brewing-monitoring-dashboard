using EHBrewingMonitoringDashboard.Enums;

namespace EHBrewingMonitoringDashboard.DTOs;

public record FermenterOverviewDto
(
    Guid Id,
    string Name,
    bool Active,
    List<SensorReadingDto> Readings
)
{
    public SensorReadingDto? Temperature => Readings.FirstOrDefault(r => r.Type == ESensorType.TEMPERATURE);
    public SensorReadingDto? Density => Readings.FirstOrDefault(r => r.Type == ESensorType.DENSITY);

    public EFermenterStatus Status =>
    Temperature switch
    {
        null => EFermenterStatus.NoData,
        { Value: >= 18 and <= 24 } => EFermenterStatus.Ok,
        { Value: >= 15 and < 18 or > 24 and <= 27 } => EFermenterStatus.Warning,
        _ => EFermenterStatus.Critical
    };
}