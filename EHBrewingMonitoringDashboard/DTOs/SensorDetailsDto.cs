namespace EHBrewingMonitoringDashboard.DTOs;

using EHBrewingMonitoringDashboard.Enums;

public record SensorDetailsDto(
    Guid Id,
    ESensorType Type,
    string SerialNumber,
    bool Active,
    DateTime InstalledAt,
    DateTime? LastMaintenanceAt,
    decimal? IdealMin,
    decimal? IdealMax,
    decimal? WarningTolerance,
    string MeasureUnit,
    decimal? LastValue,
    DateTime? LastMeasureDate
);