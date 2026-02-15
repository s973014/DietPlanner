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
    public class SubstitutionConfiguration : IEntityTypeConfiguration<Substitution>
    {
        public void Configure(EntityTypeBuilder<Substitution> builder)
        {
            builder.ToTable("substitutions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Reason)
                .IsRequired();


            builder.HasOne(x => x.OriginalMeal)
                .WithMany()
                .HasForeignKey(x => x.OriginalMealId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SubstituteMeal)
                .WithMany()
                .HasForeignKey(x => x.SubstituteMealId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
