using Microsoft.AspNetCore.Mvc;
using SPSAPI.Data;
using SPSAPI.Utilities;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		private readonly IJWTTokenGenerator _tokenGenerator;

        public ClientController(ApplicationDBContext context, IJWTTokenGenerator tokenGenerator)
        {
            _context = context;
			_tokenGenerator = tokenGenerator;
        }

        [HttpPost]
		[Route("/register")]
		public async Task<IActionResult> Register([FromBody] Client client)
		{
			if (client == null || client.User == null)
			{
				return BadRequest();
			}

			try
			{
				client.User.PasswordHash = PasswordHasher.Hash(client.User.Password!);
				await _context.User.AddAsync(client.User);
				await _context.SaveChangesAsync();

				client.UserId = client.User.Id;
				await _context.Client.AddAsync(client);
				await _context.SaveChangesAsync();

				return Ok(new { Token = _tokenGenerator.Generate(client.User.Email, "Client") });
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
