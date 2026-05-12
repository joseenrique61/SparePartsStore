using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController(ApplicationDBContext context, ILogger<UserController> logger) : ControllerBase
	{
		private readonly ApplicationDBContext _context = context;

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] KeyCloakLogin keyCloakId)
		{
			User? user = await _context.User.FirstOrDefaultAsync(u => u.KeyCloakId == keyCloakId.KeycloakId);
			if (user == null)
			{
				return BadRequest("Invalid user.");
			}

			logger.LogInformation($"User logged in. UserId: {user.Id}");
			return Ok(user);
		}
	}
}

public class KeyCloakLogin
{
	public string KeycloakId { get; set; }
}