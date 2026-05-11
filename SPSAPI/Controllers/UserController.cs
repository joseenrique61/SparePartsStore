using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSAPI.Utilities;
using SPSAPI.Utilities.JWTResponseGenerator;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController(ApplicationDBContext context, IJWTResponseGenerator responseGenerator, ILogger<UserController> logger) : ControllerBase
	{
		private readonly ApplicationDBContext _context = context;

		private readonly IJWTResponseGenerator _responseGenerator = responseGenerator;

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] KeyCloakLogin keyCloakId)
		{
			// User? user = await _context.User.FirstOrDefaultAsync(u => u.Email == userProvided.Email);

			// if (user == null || !PasswordHasher.Verify(userProvided.Password!, user.PasswordHash!))
			// {
			// 	return Unauthorized("Incorrect email or password");
			// }

			// Client? client = await _context.Client.Include(nameof(Client.User)).FirstOrDefaultAsync(c => c.User!.Email == userProvided.Email);
			// if (client != null)
			// {
			// 	return Ok(_responseGenerator.Generate(client.User!.Email, UserTypes.Client, client.Id));
			// }
			// else
			// {
			// 	return Ok(_responseGenerator.Generate(user.Email, UserTypes.Admin, user.Id));
			// }

			User? user = await _context.User.FirstOrDefaultAsync(u => u.KeyCloakId == keyCloakId.KeycloakId);
			if (user == null)
			{
				return BadRequest("Invalid user.");
			}

			return Ok(user);
		}
	}
}

public class KeyCloakLogin
{
	public string KeycloakId { get; set; }
}