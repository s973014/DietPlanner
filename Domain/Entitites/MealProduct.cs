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
        public Guid MealId { get;  set; }
        public Guid ProductId { get; set; }
        public float AmountInGrams { get;  set; }

        public Meal Meal { get;  set; }
        public Product Product { get; set; }

        public MealProduct() { } 
        public MealProduct(Guid mealId, Guid productId, float grams)
        {
            MealId = mealId;
            ProductId = productId;
            AmountInGrams = grams;
        }
        public static MealProduct Create(Meal meal, Product product, float grams)
        {
            return new MealProduct(meal.Id, product.Id, grams)
            {
                Meal = meal,
                Product = product
            };
        }
        public Nutrition CalculateNutrition()
        {
            var factor = AmountInGrams / 100;
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
