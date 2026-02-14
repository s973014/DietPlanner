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
    public class ProductServiceTests
    {
        [TestMethod]
        public async Task DeleteAsync_ProductUsedInMeal_ThrowsException()
        {
            var productId = Guid.NewGuid();

            var nutrition = Nutrition.CreateForTest(42f, 3f, 1f, 5f);
            var product = new Product(productId, "Milk", nutrition);

            var productRepoMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            productRepoMock.Setup(r => r.GetByIdAsync(productId, default)).ReturnsAsync(product);
            productRepoMock.Setup(r => r.IsUsedInAnyMealAsync(productId, default)).ReturnsAsync(true);

            var service = new ProductService(productRepoMock.Object, unitOfWorkMock.Object);

            try
            {
                await service.DeleteAsync(productId, default);
                Assert.Fail("Expected InvalidOperationException was not thrown.");
            }
            catch (InvalidOperationException)
            {
                
            }
        }


        [TestMethod]
        public async Task DeleteAsync_ProductNotUsed_RemovesAndSaves()
        {
            var productId = Guid.NewGuid();

            var nutrition = Nutrition.CreateForTest(52f, 0.3f, 0.2f, 14f);
            var product = new Product(productId, "Apple", nutrition);

            var productRepoMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            productRepoMock.Setup(r => r.GetByIdAsync(productId, default)).ReturnsAsync(product);
            productRepoMock.Setup(r => r.IsUsedInAnyMealAsync(productId, default)).ReturnsAsync(false);

            var service = new ProductService(productRepoMock.Object, unitOfWorkMock.Object);

            await service.DeleteAsync(productId, default);

            productRepoMock.Verify(r => r.Remove(product), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);


        }
    }
}
