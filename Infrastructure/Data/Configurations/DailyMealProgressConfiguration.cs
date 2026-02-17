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
    public class DailyMealProgressConfiguration : IEntityTypeConfiguration<DailyMealProgress>
    {
        public void Configure(EntityTypeBuilder<DailyMealProgress> builder)
        {
            builder.ToTable("daily_meal_progress");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                   .WithMany(u => u.MealProgresses) // указываем имя коллекции
                   .HasForeignKey(x => x.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.DailyMeal)
                   .WithMany(d => d.Progresses)
                   .HasForeignKey(x => x.DailyMealId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ReplacementMeal)
                   .WithMany()
                   .HasForeignKey(x => x.ReplacementMealId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.IsEaten).IsRequired();
        }
    }

}
