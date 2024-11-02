using SPSModels.Models;

namespace SPSAPI.Utilities.JWTResponseGenerator
{
	public interface IJWTResponseGenerator
	{
		public JWTResponse Generate(string email, string role, int clientId);
	}
}
