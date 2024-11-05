using Microsoft.AspNetCore.Mvc;
using SparePartsStoreWeb.Data.UnitOfWork;
using SparePartsStoreWeb.Utilities;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
	public class PurchaseOrderController : BaseController
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IAuthenticator _authenticator;

		public PurchaseOrderController(IUnitOfWork unitOfWork, IAuthenticator authenticator)
		{
			_unitOfWork = unitOfWork;
			_authenticator = authenticator;
		}

		public async Task<IActionResult> CartInfo()
		{
			if (!_authenticator.Authenticate(UserTypes.Client))
			{
				return RedirectToAction("Login", "Client");
			}

			PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId((int)HttpContext.Session.GetInt32("ClientId")!);

			List<string> warnings = purchaseOrder.Orders
				.Where(o => o.Amount > o.SparePart!.Stock)
				.Select(o => o.SparePart!.Name)
				.ToList();
			ViewBag.Warnings = warnings;

			return View(purchaseOrder);
		}

		public async Task<IActionResult> RemoveFromCart(int sparePartId)
		{
			int? clientId = HttpContext.Session.GetInt32("ClientId");
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
			if (!_authenticator.Authenticate(UserTypes.Client))
			{
				return RedirectToAction("Login", "Client");
			}

			return View((await _unitOfWork.PurchaseOrder
				.GetByClientId((int)HttpContext.Session.GetInt32("ClientId")!))!
				.OrderByDescending(p => p.Id));
		}

		public async Task<IActionResult> Buy()
		{
			int? clientId = HttpContext.Session.GetInt32("ClientId");
			if (clientId == null)
			{
				return RedirectToAction("Login", "Client");
			}

			PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId((int)clientId);
			purchaseOrder.PurchaseCompleted = true;
			purchaseOrder.Client = null;
			await _unitOfWork.PurchaseOrder.Update(purchaseOrder);

			List<SparePart> spareParts = (await _unitOfWork.SparePart.GetAll())!;
			foreach (SparePart sparePart in spareParts)
			{
				Order? order = purchaseOrder.Orders.FirstOrDefault(o => o.SparePartId == sparePart.Id);
				if (order == null)
				{
					continue;
				}

				sparePart.Stock -= order.Amount;
				await _unitOfWork.SparePart.Update(sparePart);
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
