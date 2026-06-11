namespace EHBrewingMonitoringDashboard.DTOs;

public record SensorReadingDto
(
    Guid Id,
    string SerialNumber,
    bool Active,
    decimal? Value,
    string? MeasureUnit,
    DateTime? RecordedAt
)
{
    public string DisplayValue => Value.HasValue ? $"{Value:F2} {MeasureUnit}" : "N/A";
}