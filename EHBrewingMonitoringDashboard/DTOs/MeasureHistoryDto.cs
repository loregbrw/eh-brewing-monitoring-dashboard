namespace EHBrewingMonitoringDashboard.DTOs;

public record MeasureHistoryDto
(
    DateTime RecordedAt,
    string SensorType,
    decimal Value,
    string Unit
);