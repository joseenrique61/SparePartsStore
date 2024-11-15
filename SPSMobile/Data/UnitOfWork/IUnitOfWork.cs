using SPSMobile.Data.Repositories.CategoryRepository;
using SPSMobile.Data.Repositories.ClientRepository;
using SPSMobile.Data.Repositories.PurchaseOrderRepository;
using SPSMobile.Data.Repositories.SparePartRepository;

namespace SPSMobile.Data.UnitOfWork
{
	public interface IUnitOfWork
	{
		public ISparePartRepository SparePart { get; }

		public ICategoryRepository Category { get; }

		public IPurchaseOrderRepository PurchaseOrder { get; }

		public IClientRepository Client { get; }
	}
}
