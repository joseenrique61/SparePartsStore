using SPSMobile.Data.ApiClient;
using SPSModels.Models;
using System.Net.Http.Json;

namespace SPSMobile.Data.Repositories.PurchaseOrderRepository
{
	public class PurchaseOrderRepository : IPurchaseOrderRepository
	{
		private readonly IApiClient _client;

		public PurchaseOrderRepository(IApiClient client)
		{
			_client = client;
		}

		public List<PurchaseOrder>? GetAll()
		{
			HttpResponseMessage response = _client.Get<PurchaseOrder>("all");
			if (response.IsSuccessStatusCode)
			{
				return response.Content.ReadFromJsonAsync<List<PurchaseOrder>>().Result;
			}
			return null;
		}

		public List<PurchaseOrder>? GetByClientId(int id)
		{
			HttpResponseMessage response = _client.Get<PurchaseOrder>($"byClientId/{id}");
			if (response.IsSuccessStatusCode)
			{
				return response.Content.ReadFromJsonAsync<List<PurchaseOrder>>().Result;
			}
			return null;
		}

		public PurchaseOrder? GetById(int id)
		{
			HttpResponseMessage response = _client.Get<PurchaseOrder>($"byId/{id}");
			if (response.IsSuccessStatusCode)
			{
				return response.Content.ReadFromJsonAsync<PurchaseOrder>().Result;
			}
			return null;
		}

		public PurchaseOrder GetCurrentByClientId(int id)
		{
			HttpResponseMessage response = _client.Get<PurchaseOrder>($"current/{id}");
			return response.Content.ReadFromJsonAsync<PurchaseOrder>().Result!;
		}

		public bool Create(PurchaseOrder purchaseOrder)
		{
			purchaseOrder.Client = null;
			
			HttpResponseMessage response = _client.Post("create", purchaseOrder);
			return response.IsSuccessStatusCode;
		}

		public bool Update(PurchaseOrder purchaseOrder)
		{
			purchaseOrder.Client = null;
			
			HttpResponseMessage response = _client.Put("update", purchaseOrder);
			return response.IsSuccessStatusCode;
		}
	}
}
