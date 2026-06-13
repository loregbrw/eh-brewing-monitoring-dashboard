namespace EHBrewingMonitoringDashboard.DTOs;

public record FermenterDetailsDto(
    Guid Id,
    string Name,
    bool Active,
    List<SensorDetailsDto> Sensors
);