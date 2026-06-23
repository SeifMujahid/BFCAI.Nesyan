using BFCAI.Nesyan.Domain.Entities.Reports;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Reports
{
    internal class ReportConfigurations:BaseEntityConfigurations<Report,int>
    {
        public override void Configure(EntityTypeBuilder<Report> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(x => x.Type)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Details)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(x => x.TargetMetrics)
                   .HasMaxLength(500);

            builder.HasOne(x => x.Patient)
                   .WithMany()
                   .HasForeignKey(x => x.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.PatientId);
        }
    }
}
