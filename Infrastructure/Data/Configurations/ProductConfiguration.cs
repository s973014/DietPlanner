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
            builder.ToTable("products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.OwnsOne(x => x.NutritionPer100g, n =>
            {
                n.Property(p => p.Calories).HasColumnName("calories");
                n.Property(p => p.Proteins).HasColumnName("proteins");
                n.Property(p => p.Fats).HasColumnName("fats");
                n.Property(p => p.Carbs).HasColumnName("carbs");
            });
        }
    }

}
