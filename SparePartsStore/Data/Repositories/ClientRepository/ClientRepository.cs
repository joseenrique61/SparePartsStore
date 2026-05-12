using SparePartsStoreWeb.Data.ApiClient;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories.ClientRepository
{
    public class ClientRepository(IApiClient client) : IClientRepository
    {
        private readonly IApiClient _client = client;

        public async Task<int> Login(string keycloakId)
        {
            HttpResponseMessage response = await _client.Post<User>("login", new KeyCloakLogin { KeyCloakId = keycloakId });

            if (!response.IsSuccessStatusCode)
            {
                return -1;
            }
            return (await response.Content.ReadFromJsonAsync<User>())!.Id;
        }

        public async Task<int> Register(Client client)
        {
            HttpResponseMessage response = await _client.Post("register", client);
            if (!response.IsSuccessStatusCode)
            {
                return -1;
            }
            return (await response.Content.ReadFromJsonAsync<User>())!.Id;
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
    public string KeyCloakId { get; set; }
}
