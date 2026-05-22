using BFCAI.Nesyan.Domain.Entities.Primary;
using BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Infrastructure.Presistence.Data.Config.Primary
{
    internal class UserConfigurations<TEntity> : BaseEntityConfigurations<TEntity, int>
        where TEntity : User
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            builder.Property(u => u.NationalId)
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(u => u.FName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Age)
                .IsRequired();

            // Enum as int
            builder.Property(u => u.Gender)
                .HasConversion<string>()
                .HasMaxLength(6)
                .IsRequired();

            builder.Property(u => u.Phone)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(u => u.MaritalStatus)
                .HasConversion<string>()
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(u => u.ImageUrl)
                .HasMaxLength(500)
                .IsRequired(false);

            // Indexes (VERY IMPORTANT)
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.UserName)
                .IsUnique();

            builder.HasIndex(u => u.NationalId)
                .IsUnique();
        }
    }
}
