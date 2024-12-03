using SPSMobile.Data.ApiClient;
using SPSModels.Models;
using System.Net.Http.Json;

namespace SPSMobile.Data.Repositories.PurchaseOrderRepository
{
	public class PurchaseOrderRepository
	{
		private readonly IApiClient _client;

		public PurchaseOrderRepository(IApiClient client)
		{
			_client = client;
		}

		public async Task<List<PurchaseOrder>?> GetAll()
		{
			HttpResponseMessage response = await _client.Get<PurchaseOrder>("all");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<PurchaseOrder>>();
			}
			return null;
		}

		public async Task<List<PurchaseOrder>?> GetByClientId(int id)
		{
			HttpResponseMessage response = await _client.Get<PurchaseOrder>($"byClientId/{id}");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<List<PurchaseOrder>>();
			}
			return null;
		}

		public async Task<PurchaseOrder?> GetById(int id)
		{
			HttpResponseMessage response = await _client.Get<PurchaseOrder>($"byId/{id}");
			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<PurchaseOrder>();
			}
			return null;
		}

		public async Task<PurchaseOrder> GetCurrentByClientId(int id)
		{
			HttpResponseMessage response = await _client.Get<PurchaseOrder>($"current/{id}");
			return (await response.Content.ReadFromJsonAsync<PurchaseOrder>())!;
		}

		public async Task<bool> Create(PurchaseOrder purchaseOrder)
		{
			HttpResponseMessage response = await _client.Post("create", purchaseOrder);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> Update(PurchaseOrder purchaseOrder)
		{
			HttpResponseMessage response = await _client.Put("update", purchaseOrder);
			return response.IsSuccessStatusCode;
		}
	}
}
