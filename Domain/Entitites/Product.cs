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
        private Product() { }

        public Product(string name, Nutrition nutritionPer100g)
        {
            Id = Guid.NewGuid();
            Name = name;
            NutritionPer100g = nutritionPer100g;
        }
    }
}
