namespace EHBrewingMonitoringDashboard.Data.Configuration;

using EHBrewingMonitoringDashboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MeasureConfiguration : IEntityTypeConfiguration<Measure>
{
    public void Configure(EntityTypeBuilder<Measure> builder)
    {
        builder.ToTable("measure");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.SensorId)
            .HasColumnName("sensor_id")
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnName("value")
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.RecordedAt)
            .HasColumnName("recorded_at")
            .IsRequired();

        builder.HasOne(x => x.Sensor)
            .WithMany(x => x.Measures)
            .HasForeignKey(x => x.SensorId);

        builder.HasIndex(x => x.SensorId);

        builder.HasIndex(x => x.RecordedAt);

        builder.HasIndex(x => new
        {
            x.SensorId,
            x.RecordedAt
        });
    }
}