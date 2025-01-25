using SQLite;

namespace SPSMobile.Utilities.Authenticator
{
	class JWTRegister
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public DateTime DateTime { get; set; } = DateTime.Now;

		public string Token { get; set; }

		public string Email { get; set; }

		public string Role { get; set; }

		public int ClientId { get; set; }
	}
}
