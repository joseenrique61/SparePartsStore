using SPSModels.Models;

namespace SPSMobile.Utilities.Authenticator
{
	public interface IAuthenticator
	{
		public JWTResponse ClientInfo { get; set; }

		public bool IsSignedIn { get; }

		public bool Authenticate(string expectedRole);
	}
}
