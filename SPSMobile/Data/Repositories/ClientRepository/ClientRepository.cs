using SPSMobile.Data.ApiClient;
using SPSMobile.Utilities.Authenticator;
using SPSModels.Models;
using System.Net.Http.Json;

namespace SPSMobile.Data.Repositories.ClientRepository
{
	public class ClientRepository : IClientRepository
	{
		private readonly IApiClient _client;

		private readonly IAuthenticator _authenticator;

		public ClientRepository(IApiClient client, IAuthenticator authenticator)
		{
			_client = client;
			_authenticator = authenticator;
		}

		public bool Login(string email, string password)
		{
			HttpResponseMessage response = _client.Post("login", new User()
			{
				Email = email,
				Password = password
			});

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			JWTResponse? token = response.Content.ReadFromJsonAsync<JWTResponse>().Result;
			if (token == null)
			{
				return false;
			}

			_client.SetToken(token.Token);
			_authenticator.ClientInfo = token;

			return true;
		}

		public bool Register(Client client)
		{
			HttpResponseMessage response = _client.Post("register", client);
			return response.IsSuccessStatusCode;
		}

		public Client? GetById(int id)
		{
			HttpResponseMessage response = _client.Get<Client>($"byId/{id}");
			if (response.IsSuccessStatusCode)
			{
				Client client = response.Content.ReadFromJsonAsync<Client>().Result!;
				return client;
			}
			return null;
		}

		public bool Logout()
		{
			_authenticator.ClientInfo = new JWTResponse
			{
				ClientId = 0,
				Email = "",
				Role = "",
				Token = ""
			};
			return true;
		}
	}
}
