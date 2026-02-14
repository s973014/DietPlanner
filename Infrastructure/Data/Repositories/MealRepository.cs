using Application.Interfaces;
using Application.Services;
using Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class MealRepository : IMealRepository
    {
        private readonly AppDbContext _context;

        public MealRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Meal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Meals
                .Include(m => m.Products)
                    .ThenInclude(mp => mp.Product)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Meal>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Meals
            .Include(m => m.Products)         
                .ThenInclude(mp => mp.Product) 
                    .ThenInclude(p => p.NutritionPer100g)
            .ToListAsync();
        }

        public async Task AddAsync(Meal meal, CancellationToken cancellationToken = default)
        {
            await _context.Meals.AddAsync(meal, cancellationToken);
        }

        public void Update(Meal meal)
        {
            _context.Meals.Update(meal);
        }
        public async Task UpdateAsync(Meal meal)
        {
            _context.Meals.Update(meal);
            await _context.SaveChangesAsync();
        }

        public void Remove(Meal meal)
        {
            _context.Meals.Remove(meal);
        }

        public async Task<List<Meal>> GetAllWithProductsAsync()
        {
            return await _context.Meals
                .Include(m => m.Products)
                    .ThenInclude(mp => mp.Product)
                .ToListAsync();
        }

        public async Task<List<MealListItemVm>> GetMealsForListAsync()
        {
            return await _context.Meals
                .Select(meal => new MealListItemVm
                {
                    Id = meal.Id,
                    Name = meal.Name,

                    Calories = _context.MealProducts
                    .Where(mp => mp.MealId == meal.Id)
                    .Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Calories * mp.AmountInGrams / 100f)),

                                    Proteins = _context.MealProducts
                    .Where(mp => mp.MealId == meal.Id)
                    .Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Proteins * mp.AmountInGrams / 100f)),

                                    Fats = _context.MealProducts
                    .Where(mp => mp.MealId == meal.Id)
                    .Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Fats * mp.AmountInGrams / 100f)),

                                    Carbs = _context.MealProducts
                    .Where(mp => mp.MealId == meal.Id)
                    .Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Carbs * mp.AmountInGrams / 100f)),
                })
                .ToListAsync();
        }

        public async Task<Meal?> GetByIdWithProductsAsync(Guid id)
        {
            return await _context.Meals
        .Include(m => m.Products)           
            .ThenInclude(mp => mp.Product)  
                .ThenInclude(p => p.NutritionPer100g) 
        .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task RemoveProductFromMealAsync(Guid mealId, Guid productId)
        {
            var mp = await _context.MealProducts
                .FirstOrDefaultAsync(x => x.MealId == mealId && x.ProductId == productId);

            if (mp != null)
            {
                _context.MealProducts.Remove(mp);
                await _context.SaveChangesAsync();
            }
        }



    }

    public class MealListItemVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public decimal Calories { get; set; }
        public decimal Proteins { get; set; }
        public decimal Fats { get; set; }
        public decimal Carbs { get; set; }
    }



    
}
