using Microsoft.AspNetCore.Mvc;
using SparePartsStoreWeb.Data.UnitOfWork;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
	public class CategoryController : BaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		public CategoryController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _unitOfWork.Category.GetAll());
		}

		public async Task<IActionResult> Details(int id)
		{
			var category = await _unitOfWork.Category
				 .GetById(id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
		{
			if (ModelState.IsValid)
			{
				await _unitOfWork.Category.Create(category);
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var category = await _unitOfWork.Category.GetById(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
		{
			if (id != category.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				await _unitOfWork.Category.Update(category);
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var category = await _unitOfWork.Category.GetById(id);
			if (category != null)
			{
				await _unitOfWork.Category.Delete(id);
			}

			return RedirectToAction(nameof(Index));
		}

		private bool CategoryExist(int id)
		{
			if (_unitOfWork.Category.GetById(id) != null)
			{
				return true;
			}
			return false;
		}
	}
}
