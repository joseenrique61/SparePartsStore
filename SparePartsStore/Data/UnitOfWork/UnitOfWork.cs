using SparePartsStoreWeb.Data.Repositories.SparePartRepository;
using SparePartsStoreWeb.Data.Repositories.CategoryRepository;
using SparePartsStoreWeb.Data.Repositories.ClientRepository;
using SparePartsStoreWeb.Data.Repositories.PurchaseOrderRepository;

namespace SparePartsStoreWeb.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
	{
		public ISparePartRepository SparePart { get; }

		public ICategoryRepository Category { get; }
		
		public IPurchaseOrderRepository PurchaseOrder { get; }

		public IClientRepository Client { get; }

		public UnitOfWork(ISparePartRepository sparePart, ICategoryRepository category, IPurchaseOrderRepository purchaseOrder, IClientRepository client)
		{
			SparePart = sparePart;
			Category = category;
			PurchaseOrder = purchaseOrder;
			Client = client;
		}
	}
}
