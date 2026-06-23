using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Relations.Primary
{
    internal class TreatmentRequestConfigurations : BaseEntityConfigurations<TreatmentRequest,int>
    {
        public override void Configure(EntityTypeBuilder<TreatmentRequest> builder)
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
            
            builder.HasOne(r => r.Caregiver)
                   .WithMany()
                   .HasForeignKey(r => r.CaregiverId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Status (Enum → string)
            builder.Property(r => r.Status)
                   .HasConversion<string>()
                   .HasDefaultValue(RequestStatus.Pending)
                   .HasMaxLength(20)
                   .IsRequired();

            // RequestDate
            builder.Property(r => r.RequestDate)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.Ignore(r => r.CreatedBy);
            builder.Ignore(r => r.CreatedOn);
            builder.Ignore(r => r.LastModifiedBy);
            builder.Ignore(r => r.LastModifiedOn);

            builder.HasIndex(x => new{x.PatientId, x.DoctorId})
            .IsUnique()
            .HasFilter("[Status] = 'Selected'");

            builder.HasIndex(x => new { x.PatientId, x.CaregiverId })
            .IsUnique()
            .HasFilter("[Status] = 'Selected'");

            builder.HasCheckConstraint(
                "CK_TreatmentRequest_Target",
                "((DoctorId IS NOT NULL AND CaregiverId IS NULL) OR (DoctorId IS NULL AND CaregiverId IS NOT NULL))");

        }
    }
}
