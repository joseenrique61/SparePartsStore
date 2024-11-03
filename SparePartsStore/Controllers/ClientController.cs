using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SparePartsStoreWeb.Data.UnitOfWork;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
    public class ClientController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("ClientId") != null && HttpContext.Session.GetString("Email") != null && HttpContext.Session.GetString("Role") != null) {
                return View(_unitOfWork.Client.Login(HttpContext.Session.GetString("Email"), HttpContext.Session.GetString("Role")));
            }

            return View();
        }

        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = new SelectList(await _unitOfWork.Category.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,City,Country")] Client client)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Client.Register(client);
                return RedirectToAction(nameof(Login));
            }
            return View(client);
        }
    }
}


