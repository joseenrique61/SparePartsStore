using System;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Name,Address,City,Country")] Client client)
        {
            if (ModelState.IsValid)
            {
                if (await _unitOfWork.Client.Register(client) == true)
                {
                    return RedirectToAction(nameof(Login));
                } 
            }
            return View(client);
        }
    }
}


