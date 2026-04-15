using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Relations.Primary
{
    internal class RelativeDoctorRequestConfigurations : IEntityTypeConfiguration<RelativeDoctorRequest>
    {
        public void Configure(EntityTypeBuilder<RelativeDoctorRequest> builder)
        {
            builder.HasKey(r => r.Id);

            // Relations
            builder.HasOne(r => r.Patient)
                   .WithMany()
                   .HasForeignKey(r => r.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Relative)
                   .WithMany()
                   .HasForeignKey(r => r.RelativeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Doctor)
                   .WithMany()
                   .HasForeignKey(r => r.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Status (Enum → string)
            builder.Property(r => r.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            // RequestDate
            builder.Property(r => r.RequestDate)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.HasIndex(r => r.PatientId)
                   .IsUnique()
                   .HasFilter("[Status] = 'Selected'");
        }
    }
}
