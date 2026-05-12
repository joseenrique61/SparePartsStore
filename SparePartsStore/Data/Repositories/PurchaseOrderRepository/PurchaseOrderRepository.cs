using SparePartsStoreWeb.Data.ApiClient;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories.PurchaseOrderRepository
{
	public class PurchaseOrderRepository(IApiClient client, ILogger<PurchaseOrderRepository> logger) : IPurchaseOrderRepository
	{
		private readonly IApiClient _client = client;

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

		public async Task<bool> Pay(string token)
		{
			HttpResponseMessage response = await _client.PostFullRoute("payment/process", new {token = token});

			logger.LogWarning(await response.Content.ReadAsStringAsync());
			// PurchaseOrder purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId((int)clientId);
			// purchaseOrder.PurchaseCompleted = true;
			// purchaseOrder.Client = null;
			// await _unitOfWork.PurchaseOrder.Update(purchaseOrder);

			// List<SparePart> spareParts = (await _unitOfWork.SparePart.GetAll())!;
			// foreach (SparePart sparePart in spareParts)
			// {
			// 	Order? order = purchaseOrder.Orders.FirstOrDefault(o => o.SparePartId == sparePart.Id);
			// 	if (order == null)
			// 	{
			// 		continue;
			// 	}

			// 	sparePart.Stock -= order.Amount;
			// 	await _unitOfWork.SparePart.Update(sparePart);
			// }

			return response.IsSuccessStatusCode;
		}
	}
}
