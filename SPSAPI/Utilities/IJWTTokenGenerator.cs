namespace SPSAPI.Utilities
{
	public interface IJWTTokenGenerator
	{
		public string Generate(string email, string role);
	}
}
