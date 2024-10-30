using SparePartsStoreWeb.Data.ApiClient;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly IApiClient _client;

		public CategoryRepository(IApiClient client)
		{
			_client = client;
		}
	}
}
