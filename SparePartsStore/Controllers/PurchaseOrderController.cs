using Microsoft.AspNetCore.Mvc;
using SparePartsStoreWeb.Data.UnitOfWork;
using SparePartsStoreWeb.Utilities;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
	public class PurchaseOrderController(IUnitOfWork unitOfWork, IAuthenticator authenticator, ILogger<PurchaseOrderController> logger, IConfiguration configuration) : BaseController
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;

		private readonly IAuthenticator _authenticator = authenticator;

		private readonly ILogger<PurchaseOrderController> _logger = logger;

    public async Task<IActionResult> CartInfo()
		{
			if (!_authenticator.Authenticate())
			{
				return RedirectToAction("Login", "Client");
			}

			PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId((int)HttpContext.User.Id()!);

			List<string> warnings = purchaseOrder.Orders
				.Where(o => o.Amount > o.SparePart!.Stock)
				.Select(o => o.SparePart!.Name)
				.ToList();
			ViewBag.Warnings = warnings;
			ViewBag.ApiUrl = configuration.GetSection("API:Url").Value;

			return View(purchaseOrder);
		}

		public async Task<IActionResult> RemoveFromCart(int sparePartId)
		{
			if (!_authenticator.Authenticate())
			{
				return RedirectToAction("Login", "Client");
			}

			int? clientId = HttpContext.User.Id();
			if (clientId == null)
			{
				return RedirectToAction("Login", "Client");
			}

			PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId((int)clientId);


			var existingOrder = purchaseOrder.Orders.FirstOrDefault(o => o.SparePartId == sparePartId);

			if (existingOrder != null)
			{

				purchaseOrder.Orders.Remove(existingOrder);
				purchaseOrder.Client = null;

				await _unitOfWork.PurchaseOrder.Update(purchaseOrder);
			}
			return RedirectToAction("CartInfo", "PurchaseOrder");
		}

		public async Task<IActionResult> PurchaseOrderListAdmin()
		{
			if (!_authenticator.Authenticate(UserTypes.Admin))
			{
				return RedirectToAction("Login", "Client");
			}

			return View((await _unitOfWork.PurchaseOrder
				.GetAll())!
				.OrderByDescending(p => p.Id));
		}

		public async Task<IActionResult> PurchaseOrderListClient()
		{
			if (!_authenticator.Authenticate())
			{
				return RedirectToAction("Login", "Client");
			}

			return View((await _unitOfWork.PurchaseOrder
				.GetByClientId((int)HttpContext.User.Id()!))!
				.OrderByDescending(p => p.Id));
		}

		public async Task<IActionResult> Buy(string token)
		{
			if (!_authenticator.Authenticate())
			{
				return RedirectToAction("Login", "Client");
			}

			_logger.LogWarning(token);		

			if (!await _unitOfWork.PurchaseOrder.Pay(token))
			{
				return BadRequest();
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
