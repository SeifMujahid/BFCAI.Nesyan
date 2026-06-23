using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Medications
{
    internal class MedicationConfigurations : BaseEntityConfigurations<Medication, int>
    {
        public override void Configure(EntityTypeBuilder<Medication> builder)
        {
            base.Configure(builder);
            builder.ToTable("Reminders");


            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Name)
                .HasMaxLength(200);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Enum Handling
            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Frequency)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.ReminderDate)
                .IsRequired();

            builder.Property(x => x.ReminderTime)
                .IsRequired();

            // Relationship With Patient
            builder.HasOne(x => x.Patient)
                .WithMany(x => x.Reminders)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

