using SparePartsStoreWeb.Data.ApiClient;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories.ClientRepository
{
    public class ClientRepository : IClientRepository
    {
        private readonly IApiClient _client;

        private readonly IHttpContextAccessor _contextAccessor;

        public ClientRepository(IApiClient client, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> Login(string email, string password)
        {
            HttpResponseMessage response = await _client.Post("login", new User()
            {
                Email = email,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            JWTResponse? token = await response.Content.ReadFromJsonAsync<JWTResponse>();
            if (token == null)
            {
                return false;
            }

            _client.SetToken(token.Token);
            _contextAccessor.HttpContext!.Session.SetString("Email", token.Email);
            _contextAccessor.HttpContext!.Session.SetString("Role", token.Role);
            _contextAccessor.HttpContext!.Session.SetInt32("ClientId", token.ClientId);

            return true;
        }

        public async Task<bool> Register(Client client)
        {
            HttpResponseMessage response = await _client.Post("register", client);
            return response.IsSuccessStatusCode;
        }

        public async Task<Client?> GetById(int id)
        {
            HttpResponseMessage response = await _client.Get<Client>($"byId/{id}");
            if (response.IsSuccessStatusCode)
            {
                Client client = (await response.Content.ReadFromJsonAsync<Client>())!;
                return client;
            }
            return null;
        }
    }
}
