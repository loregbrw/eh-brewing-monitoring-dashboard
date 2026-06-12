namespace EHBrewingMonitoringDashboard.DTOs;

using EHBrewingMonitoringDashboard.Enums;

public record MeasureHistoryDto(
    ESensorType Type,
    decimal Value,
    DateTime RecordedAt
);