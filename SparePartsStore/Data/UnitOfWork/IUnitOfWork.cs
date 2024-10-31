using SparePartsStoreWeb.Data.Repositories.SparePartRepository;
using SparePartsStoreWeb.Data.Repositories.CategoryRepository;
using SparePartsStoreWeb.Data.Repositories.ClientRepository;

namespace SparePartsStoreWeb.Data.UnitOfWork
{
    public interface IUnitOfWork
	{
		public ISparePartRepository SparePart { get; }

		public ICategoryRepository Category { get; }

		public IClientRepository Client { get; }
	}
}
