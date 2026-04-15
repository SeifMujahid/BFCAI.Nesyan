using BFCAI.Nesyan.Domain.Entities.Alerts;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Alerts
{
    internal class AlertConfigurations: BaseEntityConfigurations<Alert,int>
    {
        public override void Configure(EntityTypeBuilder<Alert> builder)
        {
            base.Configure(builder);

            builder.Property(a => a.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Category)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.Priority)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(a => a.Message)
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}
