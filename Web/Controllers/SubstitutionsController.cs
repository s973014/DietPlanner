using Application.Interfaces;
using Domain.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = "Dietitian")]
    public class SubstitutionsController : Controller
    {
        private readonly ISubstitutionRepository _repository;
        private readonly IMealRepository _mealRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubstitutionsController(
            ISubstitutionRepository repository,
            IMealRepository mealRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mealRepository = mealRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _repository.GetAllAsync();

            var vm = items.Select(x => new SubstitutionListVm
            {
                Id = x.Id,
                OriginalMeal = x.OriginalMeal,
                SubstituteMeal = x.SubstituteMeal,
                Reason = x.Reason
            }).ToList();

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Meals = await _mealRepository.GetAllAsync();
            return View(new SubstitutionEditVm());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubstitutionEditVm vm)
        {
            if (vm.OriginalProductId == vm.SubstituteProductId)
                ModelState.AddModelError("", "Блюдо не может заменять само себя");

            

            var original = await _mealRepository.GetByIdAsync(vm.OriginalProductId);
            var substitute = await _mealRepository.GetByIdAsync(vm.SubstituteProductId);

            if (original == null || substitute == null)
                return NotFound();

            var substitution = new Substitution(original, substitute, vm.Reason);

            await _repository.AddAsync(substitution);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();

            var vm = new SubstitutionEditVm
            {
                Id = entity.Id,
                OriginalProductId = entity.OriginalMeal.Id,
                SubstituteProductId = entity.SubstituteMeal.Id,
                Reason = entity.Reason
            };

            ViewBag.Meals = await _mealRepository.GetAllAsync();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubstitutionEditVm vm)
        {
            var entity = await _repository.GetByIdAsync(vm.Id);
            if (entity == null) return NotFound();

            var substitute = await _mealRepository.GetByIdAsync(vm.SubstituteProductId);
            if (substitute == null) return NotFound();

            entity.Update(substitute.Id, vm.Reason);

            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();

            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

    

    public class SubstitutionEditVm
    {
        public Guid Id { get; set; }

        public Guid OriginalProductId { get; set; }
        public Guid SubstituteProductId { get; set; }

        public string Reason { get; set; }
    }


}
