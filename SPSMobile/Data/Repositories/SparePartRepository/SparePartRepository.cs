using SPSMobile.Data.ApiClient;
using SPSModels.Models;
using System.Net.Http.Json;

namespace SPSMobile.Data.Repositories.SparePartRepository
{
	public class SparePartRepository : ISparePartRepository
	{
		private readonly IApiClient _client;

		public SparePartRepository(IApiClient client)
		{
			_client = client;
		}

		public List<SparePart>? GetAll()
		{
			HttpResponseMessage response = _client.Get<SparePart>("all");
			if (response.IsSuccessStatusCode)
			{
				return response.Content.ReadFromJsonAsync<List<SparePart>>().Result;
			}
			return null;
		}

		public SparePart? GetById(int id)
		{
			HttpResponseMessage response = _client.Get<SparePart>($"byId/{id}");
			if (response.IsSuccessStatusCode)
			{
				return response.Content.ReadFromJsonAsync<SparePart>().Result;
			}
			return null;
		}

		public List<SparePart>? GetByCategory(string categoryName)
		{
			HttpResponseMessage response = _client.Get<SparePart>($"byCategoryName/{categoryName}");
			if (response.IsSuccessStatusCode)
			{
				return response.Content.ReadFromJsonAsync<List<SparePart>>().Result;
			}
			return null;
		}

		public bool Create(SparePart sparePart)
		{
			HttpResponseMessage response = _client.Post("create", sparePart);
			return response.IsSuccessStatusCode;
		}

		public bool Update(SparePart sparePart)
		{
			HttpResponseMessage response = _client.Put("update", sparePart);
			return response.IsSuccessStatusCode;
		}

		public bool Delete(int id)
		{
			HttpResponseMessage response = _client.Delete<SparePart>($"delete/{id}");
			return response.IsSuccessStatusCode;
		}
	}
}
