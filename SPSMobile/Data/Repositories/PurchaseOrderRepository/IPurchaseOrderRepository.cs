using SPSModels.Models;

namespace SPSMobile.Data.Repositories.PurchaseOrderRepository
{
	public interface IPurchaseOrderRepository
	{
		public Task<List<PurchaseOrder>?> GetAll();

		public Task<PurchaseOrder> GetCurrentByClientId(int id);

		public Task<PurchaseOrder?> GetById(int id);

		public Task<List<PurchaseOrder>?> GetByClientId(int id);

		public Task<bool> Create(PurchaseOrder purchaseOrder);

		public Task<bool> Update(PurchaseOrder purchaseOrder);
	}
}
