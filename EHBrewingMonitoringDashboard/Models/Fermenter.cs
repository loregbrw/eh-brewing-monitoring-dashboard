namespace EHBrewingMonitoringDashboard.Models;

public class Fermenter
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? DeletedAt { get; set; }
    public required string Name { get; set; }
    public bool Active { get; set; } = true;
    public ICollection<Sensor> Sensors { get; set; } = [];
}