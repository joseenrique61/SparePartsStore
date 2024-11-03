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
			return View(purchaseOrder);
		}
	}
}
