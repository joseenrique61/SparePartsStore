namespace SparePartsStoreWeb.Data.ApiClient
{
	public class ApiClient : IApiClient
	{
		private readonly HttpClient _client;

		private readonly IHttpContextAccessor _httpContextAccessor;

		public ApiClient(HttpClient client, IHttpContextAccessor httpContextAccesor)
		{
			_client = client;

			_client.BaseAddress = new Uri("http://localhost:5027/api/");
			_httpContextAccessor = httpContextAccesor;

			SetToken(_httpContextAccessor.HttpContext!.Session.GetString("JWToken") ?? "");
		}

		private static string GetRoute<T>(string route)
		{
			return $"{typeof(T).Name}/{route}";
		}

		public void SetToken(string token)
		{
			_client.DefaultRequestHeaders.Authorization = new("Bearer", token);
			_httpContextAccessor.HttpContext!.Session.SetString("JWToken", token);
		}

		public async Task<HttpResponseMessage> Get<T>(string route)
		{
			return await _client.GetAsync(GetRoute<T>(route));
		}

		public async Task<HttpResponseMessage> Post<T>(string route, T data)
		{
			return await _client.PostAsJsonAsync(GetRoute<T>(route), data);
		}
		public async Task<HttpResponseMessage> Put<T>(string route, T data)
		{
			return await _client.PutAsJsonAsync(GetRoute<T>(route), data);
		}

		public async Task<HttpResponseMessage> Delete<T>(string route)
		{
			return await _client.DeleteAsync(GetRoute<T>(route));
		}
	}
}
