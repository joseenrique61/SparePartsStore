using SparePartsStoreWeb.Data.ApiClient;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories
{
	public class SparePartRepository : ISparePartRepository
	{
		private readonly IApiClient _client;

		public SparePartRepository(IApiClient client)
		{
			_client = client;
		}

		public async Task<List<SparePart>?> GetAll()
		{
			HttpResponseMessage response = await _client.Get<SparePart>("all");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<SparePart>>();
			}
			return null;
		}

		public async Task Create(SparePart sparePart)
		{
			var response = await _client.Post("create", sparePart);
		}
	}
}
