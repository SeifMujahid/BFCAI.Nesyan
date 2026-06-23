using BFCAI.Nesyan.Domain.Entities.Location;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Location
{
    internal class SafeZoneConfiguration : BaseEntityConfigurations<SafeZone, int>
    {
        public override void Configure(EntityTypeBuilder<SafeZone> builder)
        {
            base.Configure(builder);
            builder.ToTable("SafeZones");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Geometry)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Patient Relationship
            builder.HasOne(x => x.Patient)
                .WithMany(p => p.SafeZones)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
