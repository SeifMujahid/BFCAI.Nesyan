using BFCAI.Nesyan.Domain.Entities.Primary.Doctor;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Primary.Doctors
{
    internal class DoctorConfigurations: UserConfigurations<Doctor>
    {
        public override void Configure(EntityTypeBuilder<Doctor> builder)
        {
            base.Configure(builder);
            builder.Property(d => d.GraduationDegree)
            .IsRequired()
            .HasMaxLength(200);

            builder.Property(d => d.MedicalAssociationCard)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
