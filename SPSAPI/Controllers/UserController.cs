﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPSAPI.Data;
using SPSAPI.Utilities;
using SPSAPI.Utilities.JWTResponseGenerator;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

		private readonly IJWTResponseGenerator _responseGenerator;

        public UserController(ApplicationDBContext context, IJWTResponseGenerator responseGenerator)
        {
            _context = context;
			_responseGenerator = responseGenerator;
        }

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] User userProvided)
		{
			User? user = await _context.User.FirstOrDefaultAsync(u => u.Email == userProvided.Email);

			if (user == null || !PasswordHasher.Verify(userProvided.Password!, user.PasswordHash!))
			{
				return Unauthorized("Incorrect email or password");
			}

			Client? client = await _context.Client.Include(nameof(Client.User)).FirstOrDefaultAsync(c => c.User!.Email == userProvided.Email);
			if (client != null)
			{
				return Ok(_responseGenerator.Generate(client.User!.Email, UserTypes.Client, client.Id));
			}
			else
			{
				return Ok(_responseGenerator.Generate(user.Email, UserTypes.Admin, user.Id));
			}
		}
    }
}
