using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ILogger _logger;

		private readonly ApplicationDBContext _context;

		public CategoryController(ILogger<CategoryController> logger, ApplicationDBContext context)
		{
			_logger = logger;
			_context = context;
		}

		[HttpGet]
		[Route("all")]
		public List<Category> GetAll()
		{
			return _context.Category.ToList();
		}

		[HttpGet]
		[Route("byId/{id}")]
		public async Task<ActionResult> GetById(int id)
		{
			Category? category = await _context.Category.FindAsync(id);
			return category != null ? Ok(category) : NotFound();
		}

		[HttpGet]
		[Route("byName/{name}")]
		public async Task<ActionResult> GetByName(string name)
		{
			Category? category = await _context.Category.FirstOrDefaultAsync(c => c.Name == name);
			return category != null ? Ok(category) : NotFound();
		}

		[HttpPost]
		[Route("create")]
		[Authorize(Roles = UserTypes.Admin)]
		public async Task<ActionResult> Create([FromBody] Category category)
		{
			try
			{
				await _context.Category.AddAsync(category);
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
		public async Task<ActionResult> Update([FromBody] Category category)
		{
			try
			{
				_context.Category.Update(category);
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
				Category? category = await _context.Category.FindAsync(id);
				if (category == null)
				{
					return NotFound();
				}
				
				_context.Category.Remove(category);
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
