using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SparePartsStoreWeb.Utilities;

namespace SparePartsStoreWeb.Controllers
{
	public class BaseController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

            ViewBag.ClientId = HttpContext.User.Id().ToString() ?? "";
			ViewBag.Role = HttpContext.Session.GetString("Role") ?? "";
			ViewBag.Email = HttpContext.Session.GetString("Email") ?? "";
		}
	}
}
