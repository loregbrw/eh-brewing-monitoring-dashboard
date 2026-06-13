namespace EHBrewingMonitoringDashboard.DTOs;

public record FermenterStatusCountDto
(
    string Name,
    int Ok,
    int Warning,
    int Critical,
    int NoData
);