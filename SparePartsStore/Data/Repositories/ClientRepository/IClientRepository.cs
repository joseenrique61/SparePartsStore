namespace SparePartsStoreWeb.Data.Repositories.ClientRepository
{
    public interface IClientRepository
    {
        public Task<bool> Login(string email, string password);
    }
}
