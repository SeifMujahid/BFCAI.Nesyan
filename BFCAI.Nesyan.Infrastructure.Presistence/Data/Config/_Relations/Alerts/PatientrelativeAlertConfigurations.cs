using BFCAI.Nesyan.Domain.Entities.Relations.Alerts;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Relations.Alerts
{
    internal class PatientrelativeAlertConfigurations : BaseEntityConfigurations<PatientRelativeAlert,int>
    {
        public override void Configure(EntityTypeBuilder<PatientRelativeAlert> builder)
        {

            builder.HasKey(x => new { x.PatientId, x.RelativeId, x.AlertId });

            builder.HasOne(x => x.Patient)
                   .WithMany()
                   .HasForeignKey(x => x.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Relative)
                   .WithMany()
                   .HasForeignKey(x => x.RelativeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Alert)
                   .WithMany()
                   .HasForeignKey(x => x.AlertId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Notes)
                   .HasMaxLength(500)
                   .IsRequired();
        }
    }
}
