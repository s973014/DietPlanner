using Application.Interfaces;
using Application.Services;
using Domain.Entitites;
using Domain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Tests.Services
{
    [TestClass]
    public class MealServiceTests
    {
        [TestMethod]
        public async Task AddProductAsync_AddsProductToMeal()
        {
            var mealId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var nutrition = Nutrition.CreateForTest(155f, 13f, 11f, 1f);

            var product = new Product(productId, "Egg", nutrition);

            var meal = new Meal(mealId, "Omelette");

            var mealRepoMock = new Mock<IMealRepository>();
            var productRepoMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            mealRepoMock.Setup(r => r.GetByIdAsync(mealId, default)).ReturnsAsync(meal);
            productRepoMock.Setup(r => r.GetByIdAsync(productId, default)).ReturnsAsync(product);

            var service = new MealService(mealRepoMock.Object, productRepoMock.Object, unitOfWorkMock.Object);

            await service.AddProductAsync(mealId, productId, 100, default);

            Assert.AreEqual(1, meal.Products.Count);
            var mealProduct = meal.Products.First();
            Assert.AreEqual(productId, mealProduct.Product.Id);
            Assert.AreEqual(mealId, mealProduct.Meal.Id);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }





    }
}
