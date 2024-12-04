using SPSMobile.Data.FileManager;
using SPSModels.Models;

namespace SPSMobile.Data.Repositories.PurchaseOrderRepository
{
	internal class LocalPurchaseOrderRepository : IPurchaseOrderRepository, ILocalRepository
	{
		public string FileName { get; init; } = "purchaseOrders.json";

		private readonly FileManager<List<PurchaseOrder>> _fileManager = new();

		public List<PurchaseOrder>? GetAll()
		{
			throw new NotImplementedException();
		}

		public List<PurchaseOrder>? GetByClientId(int id)
		{
			List<PurchaseOrder>? purchaseOrders = (List<PurchaseOrder>?)_fileManager.ReadFile(FileName);
			if (purchaseOrders == null)
			{
				return null;
			}
			return purchaseOrders.Where(c => c.Id == id).ToList();
		}

		public PurchaseOrder? GetById(int id)
		{
			List<PurchaseOrder>? purchaseOrders = (List<PurchaseOrder>?)_fileManager.ReadFile(FileName);
			if (purchaseOrders == null)
			{
				return null;
			}
			return purchaseOrders.Find(c => c.Id == id);
		}

		public PurchaseOrder GetCurrentByClientId(int id)
		{
			List<PurchaseOrder>? purchaseOrders = (List<PurchaseOrder>?)_fileManager.ReadFile(FileName);
			if (purchaseOrders == null)
			{
				purchaseOrders = [new PurchaseOrder() {
					Id = 1,
					ClientId = id,
					PurchaseCompleted = false,
					Orders = []
				}];
				_fileManager.SaveFile(FileName, purchaseOrders);
				return purchaseOrders[0];
			}

			PurchaseOrder? purchaseOrder = purchaseOrders.Find(c => c.ClientId == id && c.PurchaseCompleted);
			if (purchaseOrder == null)
			{
				purchaseOrders.Add(new PurchaseOrder()
				{
					Id = purchaseOrders.Count != 0 ? purchaseOrders.Last().Id + 1 : 1,
					ClientId = id,
					PurchaseCompleted = false,
					Orders = []
				});
				return purchaseOrders.Last();
			}
			return purchaseOrder;
		}

		public bool Create(PurchaseOrder purchaseOrder)
		{
			List<PurchaseOrder>? purchaseOrders = (List<PurchaseOrder>?)_fileManager.ReadFile(FileName);
			purchaseOrders ??= [];

			purchaseOrder.Id = purchaseOrders.Count != 0 ? purchaseOrders.Last().Id + 1 : 1;
			purchaseOrder.Client = null;
			purchaseOrder.Orders = [];

			purchaseOrders.Add(purchaseOrder);

			_fileManager.SaveFile(FileName, purchaseOrders);
			return true;
		}

		public bool Update(PurchaseOrder purchaseOrder)
		{
			List<PurchaseOrder>? purchaseOrders = (List<PurchaseOrder>?)_fileManager.ReadFile(FileName);
			if (purchaseOrders == null)
			{
				return false;
			}

			PurchaseOrder? temp = purchaseOrders.Find(c => c.Id == purchaseOrder.Id);
			if (temp == null)
			{
				return false;
			}

			temp = purchaseOrder;

			_fileManager.SaveFile(FileName, purchaseOrders);
			return true;
		}
	}
}
