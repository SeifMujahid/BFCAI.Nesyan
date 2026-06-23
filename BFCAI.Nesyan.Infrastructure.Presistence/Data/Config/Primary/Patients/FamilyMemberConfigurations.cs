using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Primary.Patients
{
    internal class FamilyMemberConfigurations : BaseEntityConfigurations<FamilyMember, int>
    {
        public override void Configure(EntityTypeBuilder<FamilyMember> builder)
        {
            base.Configure(builder);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Relation)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(f => f.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(f => f.ImageUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(f => f.AudioUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasOne(f => f.Patient)
                .WithMany(p => p.FamilyMembers)
                .HasForeignKey(f => f.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
