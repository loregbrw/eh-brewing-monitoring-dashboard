namespace EHBrewingMonitoringDashboard.Models;

using EHBrewingMonitoringDashboard.Enums;

public class Sensor
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? DeletedAt { get; set; }
    public Guid FermenterId { get; set; }
    public required ESensorType Type { get; set; }
    public required string SerialNumber { get; set; }
    public required string MeasureUnit { get; set; }
    public bool Active { get; set; } = true;
    public required DateTime InstalledAt { get; set; }
    public DateTime? LastMaintenanceAt { get; set; }
    public Fermenter Fermenter { get; set; } = null!;
    public ICollection<Measure> Measures { get; set; } = [];
}