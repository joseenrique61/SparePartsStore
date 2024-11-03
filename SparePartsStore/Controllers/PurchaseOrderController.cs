using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparePartsStoreWeb.Data.UnitOfWork;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PurchaseOrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> CartInfo()
        {
            PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId((int)HttpContext.Session.GetInt32("ClientId")!);
            return View(purchaseOrder);
        }

        private bool PurchaseOrderExist(int id)
        {
            if (_unitOfWork.PurchaseOrder.GetCurrentByClientId(id)!= null)
            {
                return true;
                
            }
            return false;
        }


    }
}
