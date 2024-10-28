using System.Security.Cryptography;
using System.Text;

namespace SPSAPI.Utilities
{
	public class PasswordHasher
	{
		public static string Hash(string password)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(password);
			return Convert.ToHexString(SHA256.HashData(buffer));
		}

		public static bool Verify(string password, string hash)
		{
			return Hash(password) == hash;
		}
	}
}
