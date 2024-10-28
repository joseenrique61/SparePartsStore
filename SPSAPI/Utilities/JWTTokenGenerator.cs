using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SPSAPI.Utilities
{
	public class JWTTokenGenerator : IJWTTokenGenerator
	{
		private readonly IConfiguration _configuration;

		public JWTTokenGenerator(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string Generate(string email, string role)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, email),
				new Claim(ClaimTypes.Role, role)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Keys:JWT"]!));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials, issuer: "https://hola.com", audience: "https://hola.com");

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
