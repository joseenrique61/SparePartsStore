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

        public async Task<int> Login(string keycloakId)
        {
            // HttpResponseMessage response = await _client.Post("login", new User()
            // {
            //     Email = email,
            //     Password = password
            // });

            // if (!response.IsSuccessStatusCode)
            // {
            //     return false;
            // }

            // JWTResponse? token = await response.Content.ReadFromJsonAsync<JWTResponse>();
            // if (token == null)
            // {
            //     return false;
            // }

            // _client.SetToken(token.Token);
            // _contextAccessor.HttpContext!.Session.SetString("Email", token.Email);
            // _contextAccessor.HttpContext!.Session.SetString("Role", token.Role);
            // _contextAccessor.HttpContext!.Session.SetInt32("ClientId", token.ClientId);

            // return true;

            HttpResponseMessage response = await _client.Post<User>("login", new KeyCloakLogin {KeyCloakId = keycloakId});

            if (!response.IsSuccessStatusCode)
            {
                return -1;
            }
            return response.Content.ReadFromJsonAsync<User>().Id;
        }

        public async Task<int> Register(Client client)
        {
            HttpResponseMessage response = await _client.Post("register", client);
            if (!response.IsSuccessStatusCode)
            {
                return -1;
            }
            return response.Content.ReadFromJsonAsync<User>().Id;
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

public class KeyCloakLogin
{
    public string KeyCloakId {get; set;}
}
