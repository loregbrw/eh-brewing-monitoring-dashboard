namespace EHBrewingMonitoringDashboard.DTOs;

public record FermenterOverviewDto
(
    Guid Id,
    string Name,
    bool Active,
    List<SensorReadingDto> Readings
);