using SparePartsStoreWeb.Data.Repositories;

namespace SparePartsStoreWeb.Data.UnitOfWork
{
	public interface IUnitOfWork
	{
		public ISparePartRepository SparePart { get; }

		public ICategoryRepository Category { get; }

		public IClientRepository Client { get; }
	}
}
