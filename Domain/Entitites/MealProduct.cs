using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitites
{
    public class MealProduct
    {
        public Guid MealId { get; private set; }
        public Meal Meal { get; private set; }

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }

        public float AmountInGrams { get; private set; }

        public MealProduct(Meal meal, Product product, float grams)
        {
            Meal = meal;
            MealId = meal.Id;

            Product = product;
            ProductId = product.Id;

            AmountInGrams = grams;
        }

        public Nutrition CalculateNutrition()
        {
            var factor = AmountInGrams / 100f;
            var n = Product.NutritionPer100g;

            return new Nutrition(
                n.Calories * factor,
                n.Proteins * factor,
                n.Fats * factor,
                n.Carbs * factor
            );
        }
    }
}
