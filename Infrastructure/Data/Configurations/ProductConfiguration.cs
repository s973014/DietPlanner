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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasOne(p => p.Allergy)
                   .WithMany()
                   .HasForeignKey(p => p.AllergyId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.OwnsOne(p => p.NutritionPer100g, n =>
            {
                n.Property(x => x.Calories).HasColumnName("Calories").IsRequired();
                n.Property(x => x.Proteins).HasColumnName("Proteins").IsRequired();
                n.Property(x => x.Fats).HasColumnName("Fats").IsRequired();
                n.Property(x => x.Carbs).HasColumnName("Carbs").IsRequired();
            });
        }
    }

}
