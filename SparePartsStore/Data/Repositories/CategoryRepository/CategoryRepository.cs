using SparePartsStoreWeb.Data.ApiClient;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories.CategoryRepository
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly IApiClient _client;

		public CategoryRepository(IApiClient client)
		{
			_client = client;
		}

		public async Task<List<Category>?> GetAll()
		{
			HttpResponseMessage response = await _client.Get<Category>("all");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<Category>>();
			}
			return null;
		}

		public async Task<Category?> GetById(int id)
		{
			HttpResponseMessage response = await _client.Get<Category>($"byId/{id}");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<Category>();
			}
			return null;
		}

		public async Task<Category?> GetByName(string name)
		{
			HttpResponseMessage response = await _client.Get<Category>($"byName/{name}");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<Category>();
			}
			return null;
		}

		public async Task<bool> Create(Category category)
		{
			HttpResponseMessage response = await _client.Post("create", category);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> Update(Category category)
		{
			HttpResponseMessage response = await _client.Put("update", category);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> Delete(int id)
		{
			HttpResponseMessage response = await _client.Delete<Category>($"delete/{id}");
			return response.IsSuccessStatusCode;
		}
	}
}
