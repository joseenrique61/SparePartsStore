using Microsoft.AspNetCore.Mvc;
using SparePartsStoreWeb.Data.UnitOfWork;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
	public class ClientController : BaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		public ClientController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(User user)
		{
			if (!ModelState.IsValid)
			{
				return View(user);
			}

			if (await _unitOfWork.Client.Login(user.Email, user.Password!))
			{
                return RedirectToAction("Index", "home");
            }

			ViewData["Error"] = "Incorrect email or password.";
			return View(user);
        }

		public IActionResult Register()
		{
			return View(new Client
			{
				User = new()
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(Client client)
		{
			if (!ModelState.IsValid)
			{
				return View(client);
			}
			if (!await _unitOfWork.Client.Register(client))
			{
				ViewData["Error"] = "The user is not valid.";
				return View(client);
			}

			await _unitOfWork.Client.Login(client.User!.Email, client.User!.Password!);
			return RedirectToAction("Index", "Home");
		}
	}
}


