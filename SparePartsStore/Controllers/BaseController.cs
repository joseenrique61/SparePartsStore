using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SparePartsStoreWeb.Controllers
{
	public class BaseController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

            ViewBag.ClientId = HttpContext.Session.GetInt32("ClientId").ToString() ?? "";
			ViewBag.Role = HttpContext.Session.GetString("Role") ?? "";
			ViewBag.Email = HttpContext.Session.GetString("Email") ?? "";
		}
	}
}
