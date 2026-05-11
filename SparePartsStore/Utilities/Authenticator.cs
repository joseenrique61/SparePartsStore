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
			if (string.IsNullOrEmpty(expectedRole))
			{
				return _httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated;
			}
			return _httpContextAccessor.HttpContext!.User.HasRole(expectedRole);
		}
	}
}
