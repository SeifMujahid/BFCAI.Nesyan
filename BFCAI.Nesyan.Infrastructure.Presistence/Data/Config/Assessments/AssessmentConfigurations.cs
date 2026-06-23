using BFCAI.Nesyan.Domain.Entities.Assessments;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Assessments
{
    internal class AssessmentConfigurations:BaseEntityConfigurations<Assessment,int>
    {
        public override void Configure(EntityTypeBuilder<Assessment> builder)
        {
            base.Configure(builder);

            builder.HasKey(x => x.Id);

            // Cognitive Assessment
            builder.Property(x => x.RecognitionOfName)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.RecognitionOfPlace)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.RecognitionOfTime)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.AbilityToConcentrate)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.RecallOfRecentEvents)
                .IsRequired()
                .HasConversion<int>();

            // Psychological Status
            builder.Property(x => x.AnxietyOrStress)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.DepressionOrSadness)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Aggression)
                .IsRequired()
                .HasConversion<int>();

            // Daily Activities
            builder.Property(x => x.EatingAndDrinking)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Bathing)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Dressing)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.UsingBathroom)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.Mobility)
                .IsRequired()
                .HasConversion<int>();

            // Notes
            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => x.PatientId);


            // Relationship With Patient
            builder.HasOne(x => x.Patient)
                .WithMany(x => x.Assessments)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
