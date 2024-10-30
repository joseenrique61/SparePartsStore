using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSModels.Models;
using System.Diagnostics;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SparePartController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		public SparePartController(ApplicationDBContext context)
		{
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
		public async Task<ActionResult> Create([FromBody] SparePart sparePart)
		{
			if (sparePart == null)
			{
				return NotFound("The SparePart model is not valid.");
			}

			try
			{
				await _context.SparePart.AddAsync(sparePart);
				await _context.SaveChangesAsync();

				return Ok();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return BadRequest();
			}
		}
	}
}
