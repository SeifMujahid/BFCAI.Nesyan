using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.MindGames
{
    internal class PatternGameRecordConfigurations : BaseEntityConfigurations<PatternGameRecord, int>
    {
        public override void Configure(EntityTypeBuilder<PatternGameRecord> builder)
        {
            base.Configure(builder);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PatternLevel)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.DateTime)
                .IsRequired();

            builder.Property(x => x.Score)
                .IsRequired();

            builder.Property(x => x.Rounds)
                .IsRequired();

            builder.Property(x => x.Time)
                .IsRequired();

            // Index for faster queries
            builder.HasIndex(x => x.PatientId);

            // Relationship
            builder.HasOne(x => x.Patient)
                .WithMany()
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
