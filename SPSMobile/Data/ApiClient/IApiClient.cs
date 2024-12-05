namespace SPSMobile.Data.ApiClient
{
	public interface IApiClient
	{
		public void SetToken(string token);

		public HttpResponseMessage Get<T>(string route);

		public HttpResponseMessage Post<T>(string route, T data);

		public HttpResponseMessage Put<T>(string route, T data);

		public HttpResponseMessage Delete<T>(string route);
	}
}
