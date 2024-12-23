using Microsoft.AspNetCore.Mvc;
using SparePartsStoreWeb.Data.ApiClient;
using SparePartsStoreWeb.Data.UnitOfWork;
using SparePartsStoreWeb.Models;
using System.Diagnostics;

namespace SparePartsStoreWeb.Controllers
{
	public class HomeController : BaseController
	{
		private readonly ILogger<HomeController> _logger;

		private readonly IUnitOfWork _unitOfWork;

		private readonly IApiClient _client;
		
		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IApiClient client)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
			_client = client;
		}

        public async Task<IActionResult> Index(int? categoryId)
        {
            var spareParts = await _unitOfWork.SparePart.GetAll();

            if (categoryId.HasValue)
            {
                spareParts = spareParts.Where(s => s.CategoryId == categoryId.Value).ToList();
            }

            ViewBag.CategoryId = await _unitOfWork.Category.GetAll(); 
            return View(spareParts);
        }


        public IActionResult Privacy()
		{
			return View();
		}
        public IActionResult Contact()
        {
            return View();
        }
     
        public IActionResult Logout()
		{
			_client.SetToken("");
			HttpContext.Session.SetString("Role", "");
			HttpContext.Session.SetInt32("ClientId", -1);
			HttpContext.Session.SetString("Email", "");

			return RedirectToAction(nameof(Index));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
