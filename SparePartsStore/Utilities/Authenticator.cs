namespace SparePartsStoreWeb.Utilities
{
	public class Authenticator : IAuthenticator
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public Authenticator(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public bool Authenticate(string? expectedRole)
		{
			string? role = _httpContextAccessor.HttpContext!.Session.GetString("Role");
			if (expectedRole == null)
			{
				return role != "";
			}

			return expectedRole == role;
		}
	}
}
