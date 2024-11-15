using SPSModels.Models;

namespace SPSMobile.Utilities
{
	public interface IAuthenticator
	{
		public JWTResponse ClientInfo { get; set; }

		public bool Authenticate(string expectedRole);
	}
}
