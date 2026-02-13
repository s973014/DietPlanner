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
    public class WeeklyPlanConfiguration : IEntityTypeConfiguration<WeeklyPlan>
    {
        public void Configure(EntityTypeBuilder<WeeklyPlan> builder)
        {
            builder.ToTable("weekly_plans");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsCompleted)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey("UserId");

            builder.HasMany(x => x.DailyMeals)
                .WithOne()
                .HasForeignKey("WeeklyPlanId");
        }
    }

}
