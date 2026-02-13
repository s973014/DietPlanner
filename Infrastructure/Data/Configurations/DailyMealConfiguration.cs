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
    public class DailyMealConfiguration : IEntityTypeConfiguration<DailyMeal>
    {
        public void Configure(EntityTypeBuilder<DailyMeal> builder)
        {
            builder.ToTable("daily_meals");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DayIndex).IsRequired();

            builder.Property(x => x.MealType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.IsEaten)
                .IsRequired();

            builder.HasOne(x => x.Meal)
                .WithMany()
                .HasForeignKey("MealId");
        }
    }

}
