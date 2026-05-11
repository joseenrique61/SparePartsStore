using Microsoft.AspNetCore.Authorization;
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
	public class ClientController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		private readonly IJWTResponseGenerator _responseGenerator;

		private readonly ILogger<ClientController> _logger;

		public ClientController(ApplicationDBContext context, IJWTResponseGenerator responseGenerator, ILogger<ClientController> logger)
		{
			_context = context;
			_responseGenerator = responseGenerator;
			_logger = logger;
		}

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

				// return Ok(_responseGenerator.Generate(client.User.Email, UserTypes.Client, client.Id));
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

		[Authorize] // Ahora protege con Keycloak
		[HttpGet("profile")]
		public IActionResult GetProfile()
		{
			// El ID del usuario en Keycloak (UUID)
			var keycloakId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			return Ok(new { Message = "Autenticado en Sistema A", KeycloakId = keycloakId });
		}
	}
}
