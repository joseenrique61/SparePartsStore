using SPSMobile.Utilities.Authenticator;
using System.Net.Http.Json;

namespace SPSMobile.Data.ApiClient
{
	public class ApiClient : IApiClient
	{
		private readonly HttpClient _client;

		public ApiClient(HttpClient client, IAuthenticator authenticator)
		{
			_client = client;

			_client.BaseAddress = new Uri("http://localhost:5027/api/");

			SetToken(authenticator.ClientInfo.Token);
		}

		private static string GetRoute<T>(string route)
		{
			return $"{typeof(T).Name}/{route}";
		}

		public void SetToken(string token)
		{
			_client.DefaultRequestHeaders.Authorization = new("Bearer", token);
		}

		public HttpResponseMessage Get<T>(string route)
		{
			return _client.GetAsync(GetRoute<T>(route)).Result;
		}

		public HttpResponseMessage Post<T>(string route, T data)
		{
			return _client.PostAsJsonAsync(GetRoute<T>(route), data).Result;
		}

		public HttpResponseMessage Put<T>(string route, T data)
		{
			return _client.PutAsJsonAsync(GetRoute<T>(route), data).Result;
		}

		public HttpResponseMessage Delete<T>(string route)
		{
			return _client.DeleteAsync(GetRoute<T>(route)).Result;
		}
	}
}
