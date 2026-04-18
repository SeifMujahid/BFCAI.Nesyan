using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Primary.Patients
{
    internal class PatientConfigurations : UserConfigurations<Patient>
    {
        public override void Configure(EntityTypeBuilder<Patient> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Doctor)
                   .WithMany(d => d.Patients)
                   .HasForeignKey(p => p.DoctorId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.Property(p => p.BloodType)
                .IsRequired()
                .HasMaxLength(10);
            builder.Property(p => p.CurrentStage)
               .IsRequired()
               .HasConversion<string>();

            // Height
            builder.Property(p => p.Height)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

            // Weight
            builder.Property(p => p.Weight)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

            // BloodType
            builder.Property(p => p.BloodType)
                         .HasConversion<string>()
                         .IsRequired();

            // ChronicDisease
            builder.Property(p => p.ChronicDisease)
                   .IsRequired()
                   .HasMaxLength(200);


        }
    }
}
