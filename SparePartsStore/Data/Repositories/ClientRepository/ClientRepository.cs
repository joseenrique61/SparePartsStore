using SparePartsStoreWeb.Data.ApiClient;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories.ClientRepository
{
    public class ClientRepository : IClientRepository
    {
        private readonly IApiClient _client;

        public ClientRepository(IApiClient client)
        {
            _client = client;
        }

        public async Task<bool> Login(string email, string password)
        {
            var response = await _client.Post("login", new User()
            {
                Email = email,
                Password = password
            });

            JWTToken? token = await response.Content.ReadFromJsonAsync<JWTToken>();
            if (token == null)
            {
                return false;
            }
            _client.SetToken(token.Token);
            return true;
        }
    }
}
