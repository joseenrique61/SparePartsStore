using SPSModels.Models;

namespace SPSMobile.Data.Repositories.ClientRepository
{
	public interface IClientRepository
	{
		public bool Login(string email, string password);

		public bool Register(Client client);

		public Client? GetById(int id);

		public bool Logout();
	}
}
