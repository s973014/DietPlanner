using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitites
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public Nutrition NutritionPer100g { get; set; } = null!;

        public Guid? AllergyId { get; set; }
        public Allergy? Allergy { get; set; }
        public Product()
        {
            NutritionPer100g = new Nutrition();
        }

        public Product(string name, Nutrition nutritionPer100g, Allergy? allergy = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            NutritionPer100g = nutritionPer100g;
            Allergy = allergy;
            AllergyId = allergy?.Id;
        }

        public Product(Guid id, string name, Nutrition nutrition, Allergy? allergy = null)
        {
            Id = id;
            Name = name;
            NutritionPer100g = nutrition;
            Allergy = allergy;
            AllergyId = allergy?.Id;
        }
        public void SetNutrition(Nutrition nutrition)
        {
            NutritionPer100g = nutrition;
        }
        public void SetAllergy(Allergy? allergy)
        {
            Allergy = allergy;
            AllergyId = allergy?.Id;
        }
    }
}
