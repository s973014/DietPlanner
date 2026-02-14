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
    public class MealProductConfiguration : IEntityTypeConfiguration<MealProduct>
    {
        public void Configure(EntityTypeBuilder<MealProduct> builder)
        {
            builder.ToTable("meal_products");

            builder.HasKey(x => new { x.MealId, x.ProductId });

            builder.Property(x => x.AmountInGrams).IsRequired();

            builder.HasOne(x => x.Meal)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.MealId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                   .WithMany()
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
