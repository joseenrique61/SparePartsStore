using SPSMobile.Data.FileManager;
using SPSMobile.Utilities;
using SPSModels.Models;

namespace SPSMobile.Data.Repositories.ClientRepository
{
	internal class LocalClientRepository : IClientRepository, ILocalRepository
	{
		public string FileName { get; init; } = "clients.json";

		private readonly FileManager<List<Client>> _fileManager = new();

		private readonly IAuthenticator _authenticator;

		public LocalClientRepository(IAuthenticator authenticator)
		{
			_authenticator = authenticator;
		}

		public Client? GetById(int id)
		{
			List<Client>? clients = (List<Client>?)(List<Client>?)_fileManager.ReadFile(FileName);
			if (clients == null)
			{
				return null;
			}
			return clients.Find(c => c.Id == id);
		}

		public bool Login(string email, string password)
		{
			List<Client>? clients = (List<Client>?)_fileManager.ReadFile(FileName);
			if (clients == null)
			{
				return false;
			}

			Client? client = clients.Find(c => c.User!.Email == email && c.User!.PasswordHash == PasswordHasher.Hash(password));
			if (client == null)
			{
				return false;
			}

			_authenticator.ClientInfo = new()
			{
				Token = "",
				Email = email,
				Role = UserTypes.Client,
				ClientId = client.Id
			};

			return true;
		}

		public bool Register(Client client)
		{
			List<Client>? clients = (List<Client>?)_fileManager.ReadFile(FileName);
			clients ??= [];

			Client? temp = clients.Find(c => c.User!.Email == client.User!.Email);
			if (temp != null)
			{
				return false;
			}

			client.Id = clients.Count != 0 ? clients.Last().Id + 1 : 1;
			client.UserId = client.Id;
			client.User!.PasswordHash = PasswordHasher.Hash(client.User!.Password!);
			client.User.Password = null;

			clients.Add(client);
			_fileManager.SaveFile(FileName, clients);

			return true;
		}
	}
}
