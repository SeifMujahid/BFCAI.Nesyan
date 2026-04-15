using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Relations.Primary
{
    internal class PatientRelativeConfigurations : IEntityTypeConfiguration<PatientRelative>
    {
        public void Configure(EntityTypeBuilder<PatientRelative> builder)
        {
            builder.HasKey(pr => new { pr.PatientId, pr.RelativeId });

            builder.HasOne(pr => pr.Patient)
                .WithMany()
                .HasForeignKey(pr => pr.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pr=>pr.Relative)
                .WithMany()
                .HasForeignKey(pr => pr.RelativeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pr => pr.EnrollmentDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
