using SPSModels.Models;
using System.Text.Json;

namespace SPSMobile.Utilities.Authenticator
{
	public class Authenticator : IAuthenticator
	{
		private JWTResponse clientInfo;

		public JWTResponse ClientInfo
		{
			get { return clientInfo; }
			set
			{
				clientInfo = value;
				SaveFile();
			}
		}

		public bool IsSignedIn { get => ClientInfo.ClientId != 0; }

		public Authenticator()
		{
			ClientInfo = ReadFile();
		}

		public bool Authenticate(string? expectedRole)
		{
			if (expectedRole == null)
			{
				return ClientInfo.Role != "";
			}

			return expectedRole == ClientInfo.Role;
		}

		private JWTResponse ReadFile()
		{
			string cacheDir = FileSystem.Current.CacheDirectory;
			string filePath = Path.Combine(cacheDir, "credentials");

			if (!File.Exists(filePath))
			{
				SaveFile();
			}

			using FileStream fileStream = File.OpenRead(filePath);
			string text = File.ReadAllText(filePath);
			return JsonSerializer.Deserialize<JWTResponse>(text)!;
		}

		private void SaveFile()
		{
			string cacheDir = FileSystem.Current.CacheDirectory;
			string filePath = Path.Combine(cacheDir, "credentials");

			if (clientInfo == null)
			{
				clientInfo = new JWTResponse();
			}

			using FileStream fileStream = new(filePath, FileMode.Create);
			JsonSerializer.Serialize(fileStream, ClientInfo);
		}
	}
}
