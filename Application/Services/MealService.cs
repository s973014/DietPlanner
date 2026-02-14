using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services
{
    public class MealService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MealService(
            IMealRepository mealRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _mealRepository = mealRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddProductAsync(
            Guid mealId,
            Guid productId,
            float grams,
            CancellationToken cancellationToken)
        {
            var meal = await _mealRepository.GetByIdAsync(mealId, cancellationToken)
                ?? throw new InvalidOperationException("Meal not found");

            var product = await _productRepository.GetByIdAsync(productId, cancellationToken)
                ?? throw new InvalidOperationException("Product not found");

            meal.AddProduct(product, grams);

            _mealRepository.Update(meal);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        
    }
}
