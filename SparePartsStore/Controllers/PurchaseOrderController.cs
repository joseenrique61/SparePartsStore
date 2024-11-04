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

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(SparePart sparePart)
        {
            int? clientId = HttpContext.Session.GetInt32("ClientId");
            if (clientId == null)
            {
                return RedirectToAction("Login", "Client");
            }

            PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId((int)clientId);


            var existingOrder = purchaseOrder.Orders.FirstOrDefault(o => o.SparePartId == sparePart.Id);

            if (existingOrder != null)
            {

                purchaseOrder.Orders.Remove(existingOrder);


                await _unitOfWork.PurchaseOrder.Update(purchaseOrder);
            }
            return RedirectToAction("CartInfo", "PurchaseOrder");
        }
    }
}
