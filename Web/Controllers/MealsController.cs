using Application.Interfaces;
using Domain.Entitites;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Meals;

namespace Web.Controllers
{
    [Authorize(Roles = "Dietitian")]
    public class MealsController : Controller
    {
        private readonly IMealRepository _mealRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MealsController(
            IMealRepository mealRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _mealRepository = mealRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        
        public async Task<IActionResult> Index()
        {
            var meals = await _mealRepository.GetAllAsync();

            
            var mealsVm = meals.Select(m => new MealListItemVm
            {
                Id = m.Id,
                Name = m.Name,
                Calories = m.Products.Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Calories * mp.AmountInGrams / 100f)),
                Proteins = m.Products.Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Proteins * mp.AmountInGrams / 100f)),
                Fats = m.Products.Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Fats * mp.AmountInGrams / 100f)),
                Carbs = m.Products.Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Carbs * mp.AmountInGrams / 100f))
            }).ToList();

            return View(mealsVm);
        }

        
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _productRepository.GetAllAsync();
            return View(new MealCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(MealCreateViewModel model)
        {

            var meal = new Meal(model.Name, model.Description);

            foreach (var ingredient in model.Ingredients)
            {
                var product = await _productRepository.GetByIdAsync(ingredient.ProductId);
                if (product == null) continue;

                meal.AddProduct(product, ingredient.Grams);
            }

            await _mealRepository.AddAsync(meal);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var meal = await _mealRepository.GetByIdWithProductsAsync(id);
            if (meal == null) return NotFound();

            var vm = new MealEditVm
            {
                Id = meal.Id,
                Name = meal.Name,
                Description = meal.Description,
                Ingredients = meal.Products.Select(mp => new MealProductEditVm
                {
                    ProductId = mp.ProductId,
                    AmountInGrams = mp.AmountInGrams
                }).ToList()
            };

            
            var allProducts = await _productRepository.GetAllAsync();
            ViewBag.Products = allProducts ?? new List<Domain.Entitites.Product>();

            return View(vm);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(MealEditVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var meal = await _mealRepository.GetByIdWithProductsAsync(vm.Id);
            if (meal == null) return NotFound();

            meal.Name = vm.Name;
            meal.Description = vm.Description;

            var productIdsFromForm = vm.Ingredients.Select(i => i.ProductId).ToHashSet();
            var toRemove = meal.Products.Where(mp => !productIdsFromForm.Contains(mp.ProductId)).ToList();
            foreach (var mp in toRemove)
            {
                meal.Products.Remove(mp);
            }

            foreach (var mpVm in vm.Ingredients)
            {
                var existing = meal.Products.FirstOrDefault(mp => mp.ProductId == mpVm.ProductId);
                if (existing != null)
                {
                    existing.AmountInGrams = mpVm.AmountInGrams;
                }
                else
                {
                    var product = await _productRepository.GetByIdAsync(mpVm.ProductId);
                    if (product != null)
                    {
                        meal.Products.Add(new MealProduct
                        {
                            MealId = meal.Id,
                            ProductId = product.Id,
                            Product = product,
                            AmountInGrams = mpVm.AmountInGrams
                        });
                    }
                }
            }

            await _mealRepository.UpdateAsync(meal);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var meal = await _mealRepository.GetByIdWithProductsAsync(id);
            if (meal == null) return NotFound();

            var vm = new MealListItemVm
            {
                Id = meal.Id,
                Name = meal.Name,
                Calories = meal.Products.Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Calories * mp.AmountInGrams / 100f)),
                Proteins = meal.Products.Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Proteins * mp.AmountInGrams / 100f)),
                Fats = meal.Products.Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Fats * mp.AmountInGrams / 100f)),
                Carbs = meal.Products.Sum(mp => Convert.ToDecimal(mp.Product.NutritionPer100g.Carbs * mp.AmountInGrams / 100f))
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var meal = await _mealRepository.GetByIdAsync(id);
            if (meal == null) return NotFound();

            _mealRepository.Remove(meal);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
