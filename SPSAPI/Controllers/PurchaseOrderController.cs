using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSModels.Models;
using System.Security.Claims;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PurchaseOrderController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		private readonly ILogger _logger;

		public PurchaseOrderController(ILogger<PurchaseOrderController> logger, ApplicationDBContext context)
		{
			_context = context;
			_logger = logger;
		}

		[HttpGet]
		[Route("all")]
		[Authorize(Roles = UserTypes.Admin)]
		public async Task<List<PurchaseOrder>> GetAll()
		{
			return await _context.PurchaseOrders
				.Include(purchase => purchase.Orders)
				.ThenInclude(orders => orders.SparePart)
				.ThenInclude(spare => spare!.Category)
				.Include(purchase => purchase.Client)
				.ThenInclude(client => client!.User)
				.ToListAsync();
		}

		[HttpGet]
		[Route("current/{id}")]
		public async Task<PurchaseOrder> GetCurrentByClientId(int id)
		{
			PurchaseOrder? purchaseOrder = await _context.PurchaseOrders
				.Include(purchase => purchase.Orders)
				.ThenInclude(orders => orders.SparePart)
				.ThenInclude(spare => spare!.Category)
				.Include(purchase => purchase.Client)
				.ThenInclude(client => client!.User)
				.FirstOrDefaultAsync(p => p.Client!.Id == id && !p.PurchaseCompleted);

			if (purchaseOrder == null)
			{
				await _context.PurchaseOrders.AddAsync(new PurchaseOrder
				{
					ClientId = id,
					PurchaseCompleted = false
				});

				return (await _context.PurchaseOrders
					.Include(purchase => purchase.Client)
					.ThenInclude(client => client.User)
					.FirstOrDefaultAsync(p => p.Client!.Id == id && !p.PurchaseCompleted))!;
			}

			return purchaseOrder;
		}

		[HttpGet]
		[Route("byId/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
			string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;

			PurchaseOrder? purchaseOrder = await _context.PurchaseOrders
				.Include(purchase => purchase.Orders)
				.ThenInclude(orders => orders.SparePart)
				.ThenInclude(spare => spare!.Category)
				.Include(purchase => purchase.Client)
				.ThenInclude(client => client!.User)
				.FirstOrDefaultAsync(purchase => purchase.Id == id);

			if (purchaseOrder == null)
			{
				return NotFound();
			}

			if (purchaseOrder.Client!.User!.Email != email || role != UserTypes.Admin)
			{
				return Unauthorized();
			}

			return Ok(purchaseOrder);
		}

		[HttpGet]
		[Route("byClientId/{id}")]
		public async Task<IActionResult> GetByClientId(int id)
		{
			string email = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
			string role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;

			List<PurchaseOrder> purchaseOrders = await _context.PurchaseOrders
				.Include(purchase => purchase.Orders)
				.ThenInclude(orders => orders.SparePart)
				.ThenInclude(spare => spare!.Category)
				.Include(purchase => purchase.Client)
				.ThenInclude(client => client!.User)
				.Where(purchase => purchase.Client!.Id == id)
				.ToListAsync();

			if (purchaseOrders.Count == 0)
			{
				return NotFound();
			}

			if (purchaseOrders[0].Client!.User!.Email != email || role != UserTypes.Admin)
			{
				return Unauthorized();
			}

			return Ok(purchaseOrders);
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> Create([FromBody] PurchaseOrder purchaseOrder)
		{
			try
			{
				await _context.PurchaseOrders.AddAsync(purchaseOrder);
				await _context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest();
			}
		}

		[HttpPut]
		[Route("update")]
		public async Task<IActionResult> Update([FromBody] PurchaseOrder purchaseOrder)
		{
			try
			{
				// I spent hours on this, I love that it works
				IEnumerable<Order> ordersFromDB = _context.Orders
					.Where(o => o.PurchaseOrderId == purchaseOrder.Id);

				IEnumerable<Order> oldOrders = ordersFromDB.ExceptBy(purchaseOrder.Orders.Select(p => p.Id), p => p.Id);
				IEnumerable<Order> newOrders = purchaseOrder.Orders.ExceptBy(ordersFromDB.Select(p => p.Id), p => p.Id);

				_context.Orders.RemoveRange(oldOrders);
				await _context.SaveChangesAsync();

				await _context.Orders.AddRangeAsync(newOrders);
				await _context.SaveChangesAsync();

				purchaseOrder.Orders.Clear();

				_context.PurchaseOrders.Update(purchaseOrder);
				await _context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest();
			}
		}
	}
}
