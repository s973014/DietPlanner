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
            builder.HasKey(w => w.Id);

            builder.Property(w => w.CreatedAt)
                   .IsRequired();

            builder.HasMany(w => w.DailyMeals)
                   .WithOne(dm => dm.WeeklyPlan)
                   .HasForeignKey(dm => dm.WeeklyPlanId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
