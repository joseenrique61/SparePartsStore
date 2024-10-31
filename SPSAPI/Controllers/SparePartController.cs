using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SparePartController : ControllerBase
	{
		private readonly ILogger _logger;

		private readonly ApplicationDBContext _context;

		public SparePartController(ILogger<SparePartController> logger, ApplicationDBContext context)
		{
			_logger = logger;
			_context = context;
		}

		[HttpGet]
		[Route("all")]
		public List<SparePart> GetAll()
		{
			return _context.SparePart.Include(nameof(SparePart.Category)).ToList();
		}

		[HttpGet]
		[Route("byId/{id}")]
		public async Task<ActionResult> GetById(int id)
		{
			SparePart? sparePart = await _context.SparePart.Include(nameof(SparePart.Category))
				.FirstOrDefaultAsync(s => s.Id == id);
			return sparePart != null ? Ok(sparePart) : NotFound();
		}

		[HttpGet]
		[Route("byCategoryName/{categoryName}")]
		public ActionResult GetByCategory(string categoryName)
		{
			IEnumerable<SparePart> spareParts = _context.SparePart.Include(nameof(SparePart.Category))
				.Where(s => s.Category!.Name == categoryName);
			return spareParts.Any() ? Ok(spareParts) : NotFound();
		}

		[HttpPost]
		[Route("create")]
		[Authorize(Roles = UserTypes.Admin)]
		public async Task<ActionResult> Create([FromBody] SparePart sparePart)
		{
			try
			{
				await _context.SparePart.AddAsync(sparePart);
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
		[Authorize(Roles = UserTypes.Admin)]
		public async Task<ActionResult> Update([FromBody] SparePart sparePart)
		{
			try
			{
				_context.SparePart.Update(sparePart);
				await _context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest();
			}
		}

		[HttpDelete]
		[Route("delete/{id}")]
		[Authorize(Roles = UserTypes.Admin)]
		public async Task<ActionResult> Delete(int id)
		{
			try
			{
				SparePart? sparePart = await _context.SparePart.FindAsync(id);
				if (sparePart == null)
				{
					return NotFound();
				}
				
				_context.SparePart.Remove(sparePart);
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
