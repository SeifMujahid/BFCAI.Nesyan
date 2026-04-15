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
    internal class PatientDoctorConfigurations : IEntityTypeConfiguration<PatientDoctor>
    {
        public void Configure(EntityTypeBuilder<PatientDoctor> builder)
        {
            builder.HasKey(pd => new { pd.PatientId, pd.DoctorId });

            builder.HasOne(pd => pd.Patient)
                .WithMany()
                .HasForeignKey(pd => pd.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pd => pd.Doctor)
                .WithMany()
                .HasForeignKey(pd => pd.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pd => pd.EnrollmentDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
