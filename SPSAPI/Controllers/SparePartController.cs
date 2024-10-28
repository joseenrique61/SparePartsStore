using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPSAPI.Data;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Client")]
	public class SparePartController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		public SparePartController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("/all")]
		public ActionResult<List<SparePart>> GetAll()
		{
			return Ok(_context.SparePart.ToList());
		}
	}
}
