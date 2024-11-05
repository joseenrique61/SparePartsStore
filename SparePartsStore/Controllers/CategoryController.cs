using Microsoft.AspNetCore.Mvc;
using SparePartsStoreWeb.Data.UnitOfWork;
using SparePartsStoreWeb.Utilities;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
	public class CategoryController : BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IAuthenticator _authenticator;
		
		public CategoryController(IUnitOfWork unitOfWork, IAuthenticator authenticator)
		{
			_unitOfWork = unitOfWork;
			_authenticator = authenticator;
		}

		public async Task<IActionResult> Index()
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

			return View(await _unitOfWork.Category.GetAll());
		}

		public IActionResult Create()
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}
			
			if (ModelState.IsValid)
			{
				await _unitOfWork.Category.Create(category);
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		public async Task<IActionResult> Edit(int id)
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

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
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

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
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

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
