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
        public string Name { get; private set; }
        public Nutrition NutritionPer100g { get; private set; }

        public Guid? AllergyId { get; private set; }
        public Allergy? Allergy { get; private set; }
        private Product() { }

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

        public void SetAllergy(Allergy? allergy)
        {
            Allergy = allergy;
            AllergyId = allergy?.Id;
        }
    }
}
