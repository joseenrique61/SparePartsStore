namespace SPSMobile.Data.ApiClient
{
	public interface IApiClient
	{
		public void SetToken(string token);

		public Task<HttpResponseMessage> Get<T>(string route);

		public Task<HttpResponseMessage> Post<T>(string route, T data);

		public Task<HttpResponseMessage> Put<T>(string route, T data);

		public Task<HttpResponseMessage> Delete<T>(string route);
	}
}
