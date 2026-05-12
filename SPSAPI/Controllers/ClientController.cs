using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSAPI.Utilities;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientController(ApplicationDBContext context, ILogger<ClientController> logger) : ControllerBase
	{
		private readonly ApplicationDBContext _context = context;

		private readonly ILogger<ClientController> _logger = logger;

    [HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] Client client)
		{
			if (client == null || client.User == null || string.IsNullOrEmpty(client.User.Email) || string.IsNullOrEmpty(client.User.Password))
			{
				return Unauthorized("Invalid user");
			}

			try
			{
				client.User.PasswordHash = PasswordHasher.Hash(client.User.Password!);
				await _context.User.AddAsync(client.User);
				await _context.SaveChangesAsync();

				await _context.Client.AddAsync(client);
				await _context.SaveChangesAsync();

				return Ok(new User() { Id = client.Id });
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return BadRequest();
			}
		}

		[HttpGet]
		[Route("byId/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			Client? client = await _context.Client
				.Include(nameof(Client.User))
				.FirstOrDefaultAsync(c => c.Id == id);

			return client != null ? Ok(client) : NotFound();
		}
	}
}
