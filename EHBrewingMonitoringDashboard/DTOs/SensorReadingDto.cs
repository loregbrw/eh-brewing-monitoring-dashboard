namespace EHBrewingMonitoringDashboard.DTOs;

using EHBrewingMonitoringDashboard.Enums;

public record SensorReadingDto
(
    ESensorType Type,
    decimal? Value,
    string? MeasureUnit,
    DateTime? RecordedAt,
    EStatus Status,
    string? SerialNumber = null
)
{
    public string DisplayValue => Value.HasValue ? $"{Value:F2}{MeasureUnit}" : "N/A";
}