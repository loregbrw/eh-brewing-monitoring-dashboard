namespace EHBrewingMonitoringDashboard.Data.Configuration;

using EHBrewingMonitoringDashboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable("sensor");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");

        builder.Property(x => x.FermenterId)
            .HasColumnName("fermenter_id");

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<short>()
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(x => x.SerialNumber)
            .HasColumnName("serial_number")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.MeasureUnit)
            .HasColumnName("measure_unit")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Active)
            .HasColumnName("active")
            .IsRequired();

        builder.Property(x => x.InstalledAt)
            .HasColumnName("installed_at")
            .IsRequired();

        builder.Property(x => x.LastMaintenanceAt)
            .HasColumnName("last_maintenance_at");

        builder.HasOne(x => x.Fermenter)
            .WithMany(x => x.Sensors)
            .HasForeignKey(x => x.FermenterId);

        builder.HasMany(x => x.Measures)
            .WithOne(x => x.Sensor)
            .HasForeignKey(x => x.SensorId);

        builder.HasIndex(x => x.FermenterId);

        builder.HasIndex(x => x.SerialNumber)
            .IsUnique();

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}