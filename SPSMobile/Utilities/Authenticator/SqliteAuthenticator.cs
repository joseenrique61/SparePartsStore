using SPSModels.Models;
using SQLite;

namespace SPSMobile.Utilities.Authenticator
{
	class SqliteAuthenticator : IAuthenticator
	{
		private static string DBPath => Path.Combine(FileSystem.CacheDirectory, "credentials.db");

		private SQLiteConnection SQLiteConnection;

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

		public SqliteAuthenticator()
		{
			SQLiteConnection = new(DBPath);
			SQLiteConnection.CreateTable<JWTRegister>();

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
			List<JWTRegister> jwtRegisters = SQLiteConnection.Query<JWTRegister>("SELECT * FROM JWTRegister ORDER BY Id DESC");
			JWTRegister jwtRegister = jwtRegisters.Count > 0 ? jwtRegisters[0] : new JWTRegister();
			return new()
			{
				Token = jwtRegister.Token,
				ClientId = jwtRegister.ClientId,
				Email = jwtRegister.Email,
				Role = jwtRegister.Role
			};
		}

		private void SaveFile()
		{
			SQLiteConnection.Insert(new JWTRegister()
			{
				Token = ClientInfo.Token,
				ClientId= ClientInfo.ClientId,
				Email = ClientInfo.Email,
				Role = ClientInfo.Role
			});
		}
	}
}
