using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SparePartsStoreWeb.Data.UnitOfWork;
using SparePartsStoreWeb.Utilities;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
	public class SparePartController : BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IAuthenticator _authenticator;

		private readonly IWebHostEnvironment _webHostEnvironment;

		public SparePartController(IUnitOfWork unitOfWork, IAuthenticator authenticator, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_authenticator = authenticator;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

			ViewBag.CategoryId = await _unitOfWork.Category.GetAll();
			return View(await _unitOfWork.SparePart.GetAll());
		}

		public async Task<IActionResult> Details(int id)
		{
			var sparePart = await _unitOfWork.SparePart
					.GetById(id);
			if (sparePart == null)
			{
				return NotFound();
			}

			if (HttpContext.Session.GetString("Role") == UserTypes.Client)
			{
				PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId((int)HttpContext.Session.GetInt32("ClientId")!);
				Order? order = purchaseOrder.Orders.FirstOrDefault(o => o.SparePartId == id);
				ViewBag.AmountOnCart = order == null ? 0 : order.Amount;
			}

			return View(sparePart);
		}

		public async Task<IActionResult> DetailsList(string name)
		{
			ViewBag.CategoryId = await _unitOfWork.Category.GetAll();
			IEnumerable<SparePart>? spareParts = (await _unitOfWork.SparePart.GetAll())?
				.Where(sp => sp.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
			return spareParts == null ? NotFound() : View(spareParts);
		}

		public async Task<IActionResult> AddToCart(int amount, SparePart sparePart)
		{
			if (!_authenticator.Authenticate(UserTypes.Client))
			{
				return RedirectToAction("Login", "Client");
			}
			
			sparePart = (await _unitOfWork.SparePart.GetById(sparePart.Id))!;

			if (amount > sparePart.Stock || amount <= 0)
			{
				TempData["Error"] = "Invalid amount.";
				return RedirectToAction(nameof(Details), new { id = sparePart.Id });
			}

			int? clientId = HttpContext.Session.GetInt32("ClientId");
			if (clientId == null)
			{
				return RedirectToAction("Login", "Client");
			}

			PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId((int)clientId);
			Order? order = purchaseOrder.Orders.FirstOrDefault(o => o.SparePartId == sparePart.Id);
			if (order == null)
			{
				purchaseOrder.Orders.Add(new Order
				{
					SparePartId = sparePart.Id,
					Amount = amount
				});
			}
			else
			{
				if (order.Amount + amount > sparePart.Stock)
				{
					TempData["Error"] = "Invalid amount.";
					return RedirectToAction(nameof(Details), new { id = sparePart.Id });
				}
				order.Amount += amount;
			}

			purchaseOrder.Client = null;
			await _unitOfWork.PurchaseOrder.Update(purchaseOrder);

			return RedirectToAction("CartInfo", "PurchaseOrder");
		}

		public async Task<IActionResult> Create()
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

			ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Description,Stock,Price,Image,CategoryId")] SparePart sparePart)
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}
			
			IFormFile file = HttpContext.Request.Form.Files[0];

			string fileName = Guid.NewGuid().ToString();
			string imagePath = $@"\images\{fileName}{Path.GetExtension(file.FileName)}";
			string fullPath = _webHostEnvironment.WebRootPath + imagePath;

			sparePart.Image = imagePath;

			ModelState.ClearValidationState("Image");
			if (!TryValidateModel(sparePart, "Image"))
			{
				ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Name");
				return View(sparePart);
			}

			using (FileStream fileStream = new(fullPath, FileMode.Create))
			{
				file.CopyTo(fileStream);
			}

			await _unitOfWork.SparePart.Create(sparePart);

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit(int id)
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}
			
			ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Name");
			var sparePart = await _unitOfWork.SparePart.GetById(id);
			if (sparePart == null)
			{
				return NotFound();
			}
			return View(sparePart);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Stock,Price,CategoryId")] SparePart sparePart)
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

			if (id != sparePart.Id)
			{
				return NotFound();
			}

			IFormFileCollection files = HttpContext.Request.Form.Files;
			if (files.Count > 0)
			{
				IFormFile file = files[0];

				string fileName = Guid.NewGuid().ToString();
				string imagePath = $@"\images\{fileName}{Path.GetExtension(file.FileName)}";
				string fullPath = _webHostEnvironment.WebRootPath + imagePath;

				sparePart.Image = imagePath;

				ModelState.ClearValidationState("Image");
				if (!TryValidateModel(sparePart, "Image"))
				{
					ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Name");
					return View(sparePart);
				}

				using (FileStream fileStream = new(fullPath, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}
			}
			else
			{
				sparePart.Image = (await _unitOfWork.SparePart.GetById(id))!.Image;

				ModelState.ClearValidationState("Image");
				if (!TryValidateModel(sparePart, "Image"))
				{
					ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Name");
					return View(sparePart);
				}
			}

			await _unitOfWork.SparePart.Update(sparePart);
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int id)
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

			await _unitOfWork.SparePart.Delete(id);
			return RedirectToAction(nameof(Index));
		}

		private bool SparePartExist(int id)
		{
			if (_unitOfWork.SparePart.GetById(id) != null)
			{
				return true;
			}
			return false;
		}
	}
}
