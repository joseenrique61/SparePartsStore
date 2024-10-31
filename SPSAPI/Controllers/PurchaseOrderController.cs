using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PurchaseOrderController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		public PurchaseOrderController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("all")]
		[Authorize(Roles = UserTypes.Admin)]
		public async Task<List<PurchaseOrder>> GetAll()
		{
			return await _context.PurchaseOrders.Include($"{nameof(PurchaseOrder.Orders)}.{nameof(PurchaseOrder.Client)}").ToListAsync();
		}

		//getbyid
		//getbyclientid
		//create
		//udpate
	}
}
