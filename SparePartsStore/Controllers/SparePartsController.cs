using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SparePartsStore.Data;
using SparePartsStore.Models;

namespace SparePartsStore.Controllers
{
    public class SparePartsController : Controller
    {
        private readonly SparePartsStoreContext _context;

        public SparePartsController(SparePartsStoreContext context)
        {
            _context = context;
        }

        // GET: SpareParts
        public async Task<IActionResult> Index()
        {
            return View(await _context.SparePart.ToListAsync());
        }

        // GET: SpareParts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sparePart = await _context.SparePart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }

        // GET: SpareParts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SpareParts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Stock")] SparePart sparePart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sparePart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sparePart);
        }

        // GET: SpareParts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sparePart = await _context.SparePart.FindAsync(id);
            if (sparePart == null)
            {
                return NotFound();
            }
            return View(sparePart);
        }

        // POST: SpareParts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Stock")] SparePart sparePart)
        {
            if (id != sparePart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sparePart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SparePartExists(sparePart.Id))
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

        // GET: SpareParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sparePart = await _context.SparePart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }

        // POST: SpareParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sparePart = await _context.SparePart.FindAsync(id);
            if (sparePart != null)
            {
                _context.SparePart.Remove(sparePart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SparePartExists(int id)
        {
            return _context.SparePart.Any(e => e.Id == id);
        }
    }
}
