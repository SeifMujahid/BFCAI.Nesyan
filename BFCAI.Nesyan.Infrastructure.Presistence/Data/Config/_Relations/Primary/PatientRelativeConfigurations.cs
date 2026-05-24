using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
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
    internal class PatientRelativeConfigurations : BaseEntityConfigurations<PatientRelative,int>
    {
        
        public override void Configure(EntityTypeBuilder<PatientRelative> builder)
        {
            base.Configure(builder);
            builder.Ignore(pr => pr.Id);
            builder.HasKey(pr => new { pr.PatientId, pr.RelativeId });

            builder.HasOne(pr => pr.Patient)
                .WithMany(p => p.PatientRelatives)
                .HasForeignKey(pr => pr.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pr => pr.Relative)
                .WithMany(r=>r.Patients)
                .HasForeignKey(pr => pr.RelativeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pr => pr.EnrollmentDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
