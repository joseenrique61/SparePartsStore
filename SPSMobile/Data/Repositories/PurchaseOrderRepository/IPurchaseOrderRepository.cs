using SPSModels.Models;

namespace SPSMobile.Data.Repositories.PurchaseOrderRepository
{
	public interface IPurchaseOrderRepository
	{
		public List<PurchaseOrder>? GetAll();

		public PurchaseOrder GetCurrentByClientId(int id);

		public PurchaseOrder? GetById(int id);

		public List<PurchaseOrder>? GetByClientId(int id);

		public bool Create(PurchaseOrder purchaseOrder);

		public bool Update(PurchaseOrder purchaseOrder);
	}
}
