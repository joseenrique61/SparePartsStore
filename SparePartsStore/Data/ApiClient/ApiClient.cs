using Microsoft.AspNetCore.Authentication;

namespace SparePartsStoreWeb.Data.ApiClient
{
	public class ApiClient : IApiClient
	{
		private readonly HttpClient _client;

		private readonly IHttpContextAccessor _httpContextAccessor;

		public ApiClient(HttpClient client, IHttpContextAccessor httpContextAccesor, IConfiguration configuration)
		{
			_client = client;

			_client.BaseAddress = new Uri($"{configuration.GetSection("API:Url").Value}/api/");
			_httpContextAccessor = httpContextAccesor;
		}

		private static string GetRoute<T>(string route)
		{
			return $"{typeof(T).Name}/{route}";
		}

		public void SetToken(string token)
		{
			_client.DefaultRequestHeaders.Authorization = new("Bearer", token);
			// _httpContextAccessor.HttpContext!.Session.SetString("JWToken", token);
		}

		public async Task<HttpResponseMessage> Get<T>(string route)
		{
			SetToken(await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token") ?? "");
			return await _client.GetAsync(GetRoute<T>(route));
		}

		public async Task<HttpResponseMessage> Post<T>(string route, T data)
		{
			SetToken(await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token") ?? "");
			return await _client.PostAsJsonAsync(GetRoute<T>(route), data);
		}

		public async Task<HttpResponseMessage> Post<T>(string route, object data)
		{
			SetToken(await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token") ?? "");
			return await _client.PostAsJsonAsync(GetRoute<T>(route), data);
		}

		public async Task<HttpResponseMessage> PostFullRoute(string route, object data)
		{
			SetToken(await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token") ?? "");
			return await _client.PostAsJsonAsync(route, data);
		}

		public async Task<HttpResponseMessage> Put<T>(string route, T data)
		{
			SetToken(await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token") ?? "");
			return await _client.PutAsJsonAsync(GetRoute<T>(route), data);
		}

		public async Task<HttpResponseMessage> Delete<T>(string route)
		{
			SetToken(await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token") ?? "");
			return await _client.DeleteAsync(GetRoute<T>(route));
		}
	}
}
