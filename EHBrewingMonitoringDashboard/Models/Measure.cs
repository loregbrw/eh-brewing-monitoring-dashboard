namespace EHBrewingMonitoringDashboard.Models;

public class Measure
{
    public long Id { get; init; }
    public required Guid SensorId { get; set; }
    public required decimal Value { get; set; }
    public required DateTime RecordedAt { get; set; }
    public required Sensor Sensor { get; set; }
}