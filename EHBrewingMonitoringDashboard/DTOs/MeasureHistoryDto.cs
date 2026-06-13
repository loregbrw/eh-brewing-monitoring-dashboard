using EHBrewingMonitoringDashboard.Enums;

namespace EHBrewingMonitoringDashboard.DTOs;

public record MeasureHistoryDto
(
    DateTime RecordedAt,
    ESensorType SensorType,
    decimal Value,
    string Unit
);