using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparePartsStoreWeb.Data.UnitOfWork;

namespace SparePartsStoreWeb.Controllers
{
    public class SparePartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SparePartController(IUnitOfWork unitOfWork) { 
            _unitOfWork = unitOfWork;
        }

        // GET: SparePart
        public async Task<IActionResult> Index() {
            //return View(await _unitOfWork.SparePart.ToListAsync());
            return View();
        }

        // GET: Estadios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View();
            /** var SparePart = await _unitOfWork.SparePart
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (SparePart == null)
             {
                 return NotFound();
             }

             return View(SparePart);**/
        }

    }
}
