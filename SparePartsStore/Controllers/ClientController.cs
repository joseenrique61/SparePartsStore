using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using SparePartsStoreWeb.Data.UnitOfWork;
using SPSModels.Models;

namespace SparePartsStoreWeb.Controllers
{
	public class ClientController : BaseController
	{
		public IActionResult Login()
		{
			if (User.Identity?.IsAuthenticated ?? false) return RedirectToAction("Index", "Home");

			// Esto redirige automáticamente a la pantalla de Keycloak
			return Challenge(new AuthenticationProperties { RedirectUri = "/" },
					OpenIdConnectDefaults.AuthenticationScheme);
		}
	}
}


