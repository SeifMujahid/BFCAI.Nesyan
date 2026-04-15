
using BFCAI.Nesyan.Domain.Entities.Primary.Doctor;
using BFCAI.Nesyan.Domain.Entities.Primary.Patient;
using BFCAI.Nesyan.Domain.Entities.Relations.MindGames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Relations.MindGames;

public class MindGameSessionConfigurations : IEntityTypeConfiguration<MindGameSession>
{
    public void Configure(EntityTypeBuilder<MindGameSession> builder)
    {
        builder.HasOne(x => x.Doctor)
               .WithMany()
               .HasForeignKey(x => x.DoctorId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Patient)
               .WithMany()
               .HasForeignKey(x => x.PatientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.MindGame)
               .WithMany()
               .HasForeignKey(x => x.MindGameId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.AddedDate)
               .HasDefaultValueSql("GETUTCDATE()")
               .IsRequired();

        builder.Property(x => x.StartDate)
               .IsRequired();

        builder.Property(x => x.Frequency)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(x => new { x.PatientId, x.MindGameId });
    }
}

