using Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMealRepository
    {
        Task<Meal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Meal>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Meal meal, CancellationToken cancellationToken = default);
        public Task<Meal> GetByIdWithProductsAsync(Guid id);
        public Task RemoveProductFromMealAsync(Guid mealId, Guid productId);
        void Update(Meal meal);
        public Task UpdateAsync(Meal meal);
        void Remove(Meal meal);
    }
    public class MealDetailsVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<MealProductVm> Products { get; set; } = new();

        public decimal TotalCalories { get; set; }
        public decimal TotalProteins { get; set; }
        public decimal TotalFats { get; set; }
        public decimal TotalCarbs { get; set; }
    }

    public class MealProductVm
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double AmountInGrams { get; set; }

        public decimal Calories { get; set; }
        public decimal Proteins { get; set; }
        public decimal Fats { get; set; }
        public decimal Carbs { get; set; }
    }
}
