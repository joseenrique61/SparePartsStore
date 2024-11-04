using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePartsStoreWeb.Data.UnitOfWork;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
    public class BuyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BuyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Buy(int id)
        {
            var sparePart = await _unitOfWork.SparePart
                             .GetById(id);

            if (sparePart == null)
            {
                return NotFound();
            }

            ViewBag.Id = sparePart.Id;
            ViewBag.Name = sparePart.Name;
            ViewBag.Description = sparePart.Description;
            ViewBag.Stock = sparePart.Stock;
            ViewBag.Category = sparePart?.Category.Name;
            ViewBag.Price = sparePart.Price;


            return View(new PurchaseOrder());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy([Bind("Id,PurchaseOrderId,SparePartId,Ammount")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.PurchaseOrder.Create(purchaseOrder);
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseOrder);
        }

    }
}
