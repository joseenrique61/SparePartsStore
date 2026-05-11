using SparePartsStoreWeb.Data.ApiClient;
using SparePartsStoreWeb.Data.UnitOfWork;
using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories.PurchaseOrderRepository
{
	public class PurchaseOrderRepository : IPurchaseOrderRepository
	{
		private readonly IApiClient _client;

		private readonly IUnitOfWork _unitOfWork;

		public PurchaseOrderRepository(IApiClient client, IUnitOfWork unitOfWork)
		{
			_client = client;
			_unitOfWork = unitOfWork;
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

		public async Task<bool> Pay(string token)
		{
			HttpResponseMessage response = await _client.Post("payment/process", token);

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
