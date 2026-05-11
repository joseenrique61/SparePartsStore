using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories.ClientRepository
{
    public interface IClientRepository
    {
        public Task<int> Login(string keyCloakId);
        public Task<int> Register(Client client);

        public Task<Client?> GetById(int id);
    }
}
