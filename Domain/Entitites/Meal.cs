using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitites
{
    public class Meal : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private List<MealProduct> _products = new();
        public IReadOnlyCollection<MealProduct> Products => _products;

        public Nutrition TotalNutrition =>
            _products
                .Select(p => p.CalculateNutrition())
                .Aggregate(new Nutrition(0, 0, 0, 0), (a, b) => a + b);


        public Meal(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        private Meal() { }

        public Meal(Guid id, string name)
        {
            Id = id;
            Name = name;
            _products = new List<MealProduct>();
        }

        public void AddProduct(Product product, float grams)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (grams <= 0) throw new ArgumentException("Amount must be positive", nameof(grams));

            _products.Add(MealProduct.Create(this, product, grams));
        }
    }
}
