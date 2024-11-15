using SPSMobile.Data.Repositories.CategoryRepository;
using SPSMobile.Data.Repositories.ClientRepository;
using SPSMobile.Data.Repositories.PurchaseOrderRepository;
using SPSMobile.Data.Repositories.SparePartRepository;

namespace SPSMobile.Data.UnitOfWork
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
