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
		
        public ClientController(ApplicationDBContext context, IJWTResponseGenerator responseGenerator)
        {
            _context = context;
			_responseGenerator = responseGenerator;
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

				return Ok(_responseGenerator.Generate(client.User.Email, UserTypes.Client, client.Id));
			}
			catch
			{
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
