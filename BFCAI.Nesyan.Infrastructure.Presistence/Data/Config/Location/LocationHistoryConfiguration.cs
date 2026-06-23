using BFCAI.Nesyan.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Location
{
    internal class LocationHistoryConfiguration : IEntityTypeConfiguration<LocationHistory>
    {
        public void Configure(EntityTypeBuilder<LocationHistory> builder)
        {
            builder.ToTable("LocationHistories");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Lat)
                .IsRequired();

            builder.Property(x => x.Lng)
                .IsRequired();

            builder.Property(x => x.RecordedAt)
                .IsRequired();

            // Patient Relationship
            builder.HasOne(x => x.Patient)
                .WithMany(p => p.LocationHistories)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
