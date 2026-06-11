namespace EHBrewingMonitoringDashboard.Data.Configuration;

using EHBrewingMonitoringDashboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FermenterConfiguration : IEntityTypeConfiguration<Fermenter>
{
    public void Configure(EntityTypeBuilder<Fermenter> builder)
    {
        builder.ToTable("fermenter");

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

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Active)
            .HasColumnName("active")
            .IsRequired();

        builder.HasMany(x => x.Sensors)
            .WithOne(x => x.Fermenter)
            .HasForeignKey(x => x.FermenterId);

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}