using SparePartsStoreWeb.Data.Repositories;

namespace SparePartsStoreWeb.Data.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		public ISparePartRepository SparePart { get; } = new SparePartRepository();

		public ICategoryRepository Category { get; } = new CategoryRepository();

		public IClientRepository Client { get; } = new ClientRepository();
	}
}
