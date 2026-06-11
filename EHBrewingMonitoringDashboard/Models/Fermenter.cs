namespace FermentationDashboard.Models;

public class Fermenter
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public bool Active { get; set; }

    public ICollection<Sensor> Sensors { get; set; } = [];
}