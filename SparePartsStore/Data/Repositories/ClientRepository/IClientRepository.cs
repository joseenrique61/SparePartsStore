using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories.ClientRepository
{
    public interface IClientRepository
    {
        public Task<bool> Login(string email, string password);
        public Task<bool> Register(Client client);
    }
}
