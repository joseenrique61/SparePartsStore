using SPSMobile.Data.ApiClient;
using SPSModels.Models;
using System.Net.Http.Json;

namespace SPSMobile.Data.Repositories.CategoryRepository
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly IApiClient _client;

		public CategoryRepository(IApiClient client)
		{
			_client = client;
		}

		public List<Category>? GetAll()
		{
			HttpResponseMessage response = _client.Get<Category>("all");
			if (response.IsSuccessStatusCode)
			{
				return response.Content.ReadFromJsonAsync<List<Category>>().Result;
			}
			return null;
		}

		public Category? GetById(int id)
		{
			HttpResponseMessage response = _client.Get<Category>($"byId/{id}");
			if (response.IsSuccessStatusCode)
			{
				return response.Content.ReadFromJsonAsync<Category>().Result;
			}
			return null;
		}

		public Category? GetByName(string name)
		{
			HttpResponseMessage response = _client.Get<Category>($"byName/{name}");
			if (response.IsSuccessStatusCode)
			{
				return response.Content.ReadFromJsonAsync<Category>().Result;
			}
			return null;
		}

		public bool Create(Category category)
		{
			HttpResponseMessage response = _client.Post("create", category);
			return response.IsSuccessStatusCode;
		}

		public  bool Update(Category category)
		{
			HttpResponseMessage response = _client.Put("update", category);
			return response.IsSuccessStatusCode;
		}

		public  bool Delete(int id)
		{
			HttpResponseMessage response = _client.Delete<Category>($"delete/{id}");
			return response.IsSuccessStatusCode;
		}
	}
}
