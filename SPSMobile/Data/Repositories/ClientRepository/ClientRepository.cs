using SPSMobile.Data.ApiClient;
using SPSMobile.Utilities;
using SPSModels.Models;
using System.Net.Http.Json;

namespace SPSMobile.Data.Repositories.ClientRepository
{
	public class ClientRepository
	{
		private readonly IApiClient _client;

		private readonly IAuthenticator _authenticator;

		public ClientRepository(IApiClient client, IAuthenticator authenticator)
		{
			_client = client;
			_authenticator = authenticator;
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
			_authenticator.ClientInfo = token;

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
