using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SparePartsStoreWeb.Data.UnitOfWork;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
    public class SparePartController : BaseController
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
            var sparePart = await _unitOfWork.SparePart
                 .GetById(id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _unitOfWork.Category.GetAll(),"Id","Name");
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
            var sparePart = await _unitOfWork.SparePart.GetById(id);
            if (sparePart == null)
            {
                return NotFound();
            }
            return View(sparePart);
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
            var sparePart = await _unitOfWork.SparePart
                 .GetById(id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sparePart = await _unitOfWork.SparePart.GetById(id);
            if (sparePart != null)
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
