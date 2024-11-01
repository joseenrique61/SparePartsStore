using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparePartsStoreWeb.Data.UnitOfWork;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
    public class SparePartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SparePartController(IUnitOfWork unitOfWork) { 
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index() 
        {
            return View(await _unitOfWork.SparePart.GetAll());
        }

        public async Task<IActionResult> Details(int id)
        {
            var SparePart = await _unitOfWork.SparePart
                 .GetById(id);
            if (SparePart == null)
            {
                return NotFound();
            }

            return View(SparePart);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Stock,Image,CategoryId")] SparePart sparePart)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.SparePart.Create(sparePart);
                return RedirectToAction(nameof(Index));
            }
            return View(sparePart);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var estadio = await _unitOfWork.SparePart.GetById(id);
            if (estadio == null)
            {
                return NotFound();
            }
            return View(estadio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Stock,Image,CategoryId")] SparePart sparePart)
        {
            if (id != sparePart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.SparePart.Update(sparePart);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SparePartExist(sparePart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sparePart);
        }

        public async Task<IActionResult> Delete(int id)
        {
            /*
             if (id == null)
            {
                return NotFound();
            }
            */

            var SparePart = await _unitOfWork.SparePart
                 .GetById(id);
            if (SparePart == null)
            {
                return NotFound();
            }

            return View(SparePart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadio = await _unitOfWork.SparePart.GetById(id);
            if (estadio != null)
            {
                await _unitOfWork.SparePart.Delete(id);
            }

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
