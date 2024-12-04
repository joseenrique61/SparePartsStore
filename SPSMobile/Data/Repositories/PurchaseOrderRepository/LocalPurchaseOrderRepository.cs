using SPSMobile.Data.FileManager;
using SPSMobile.Data.Repositories.SparePartRepository;
using SPSModels.Models;

namespace SPSMobile.Data.Repositories.PurchaseOrderRepository
{
	internal class LocalPurchaseOrderRepository : IPurchaseOrderRepository, ILocalRepository
	{
		public string FileName { get; init; } = "purchaseOrders.json";

		private readonly FileManager<List<PurchaseOrder>> _fileManager = new();

		private readonly ISparePartRepository _sparePartRepository;

		public LocalPurchaseOrderRepository(ISparePartRepository sparePartRepository)
		{
			_sparePartRepository = sparePartRepository;
		}

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

			IEnumerable<PurchaseOrder> x = purchaseOrders.Where(c => c.Id == id);
			foreach (PurchaseOrder purchaseOrder in x)
			{
				foreach (Order order in purchaseOrder.Orders)
				{
					order.SparePart = _sparePartRepository.GetById(order.SparePartId);
				}
			}

			return x.ToList();
		}

		public PurchaseOrder? GetById(int id)
		{
			List<PurchaseOrder>? purchaseOrders = (List<PurchaseOrder>?)_fileManager.ReadFile(FileName);
			if (purchaseOrders == null)
			{
				return null;
			}

			PurchaseOrder? purchaseOrder = purchaseOrders.Find(c => c.Id == id);
			if (purchaseOrder == null)
			{
				return null;
			}

			foreach (Order order in purchaseOrder.Orders)
			{
				order.SparePart = _sparePartRepository.GetById(order.SparePartId);
			}

			return purchaseOrder;
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

			PurchaseOrder? purchaseOrder = purchaseOrders.Find(c => c.ClientId == id && !c.PurchaseCompleted);
			if (purchaseOrder == null)
			{
				purchaseOrders.Add(new PurchaseOrder()
				{
					Id = purchaseOrders.Count != 0 ? purchaseOrders.Last().Id + 1 : 1,
					ClientId = id,
					PurchaseCompleted = false,
					Orders = []
				});
				_fileManager.SaveFile(FileName, purchaseOrders);
				return purchaseOrders.Last();
			}

			foreach (Order order in purchaseOrder.Orders)
			{
				order.SparePart = _sparePartRepository.GetById(order.SparePartId);
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

			foreach (Order order in purchaseOrder.Orders)
			{
				order.SparePart = null;
			}
			
			int index = purchaseOrders.IndexOf(temp);
			purchaseOrders.Remove(temp);
			purchaseOrders.Insert(index, purchaseOrder);

			_fileManager.SaveFile(FileName, purchaseOrders);
			return true;
		}
	}
}
