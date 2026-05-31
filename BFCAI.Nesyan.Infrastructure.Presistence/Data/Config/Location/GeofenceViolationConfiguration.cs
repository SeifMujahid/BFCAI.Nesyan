using BFCAI.Nesyan.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Location
{
    internal class GeofenceViolationConfiguration : IEntityTypeConfiguration<GeofenceViolation>
    {
        public void Configure(EntityTypeBuilder<GeofenceViolation> builder)
        {
            builder.ToTable("GeofenceViolations");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.ExitLat)
                .IsRequired();

            builder.Property(x => x.ExitLng)
                .IsRequired();

            builder.Property(x => x.ExitedAt)
                .IsRequired();

            builder.Property(x => x.EnteredAt)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(15);

            builder.Property(x => x.NotificationSent)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships (using Restrict to avoid multiple cascade paths in SQL Server)
            builder.HasOne(x => x.Patient)
                .WithMany(p => p.GeofenceViolations)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SafeZone)
                .WithMany(s => s.GeofenceViolations)
                .HasForeignKey(x => x.SafeZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
