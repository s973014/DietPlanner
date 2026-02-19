using Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(150);
            builder.Property(x => x.PasswordHash).IsRequired();

            builder.Property(x => x.Role)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.ActivityLevel)
                .HasConversion<string>();

            builder.Property(x => x.Goal)
                .HasConversion<string>();

            builder.Property(x => x.CurrentPlanDay)
               .IsRequired()
               .HasDefaultValue(0);

            builder.HasMany(x => x.Allergies)
                .WithMany()
                .UsingEntity(j =>
                {
                    j.ToTable("user_allergies");
                });
            builder.HasOne(x => x.WeeklyPlan)
           .WithMany()
           .HasForeignKey("WeeklyPlanId")
           .OnDelete(DeleteBehavior.SetNull);
        }
    }

}
