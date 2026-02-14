using Application.Interfaces;
using Domain.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data.Repositories;
namespace Web.Controllers
{
    [Authorize(Roles = "Dietitian")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IAllergyRepository _allergyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(
            IProductRepository productRepository,
            IAllergyRepository allergyRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _allergyRepository = allergyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllWithAllergyAsync();
            return View(products);
        }

  
        public async Task<IActionResult> Create()
        {
            ViewBag.Allergies = await _allergyRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            


            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Allergies = await _allergyRepository.GetAllAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Allergies = await _allergyRepository.GetAllAsync();
                return View(product);
            }

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            _productRepository.Remove(product);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
