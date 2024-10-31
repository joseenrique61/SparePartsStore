using SparePartsStoreWeb.Data.Repositories.SparePartRepository;
using SparePartsStoreWeb.Data.Repositories.CategoryRepository;
using SparePartsStoreWeb.Data.Repositories.ClientRepository;

namespace SparePartsStoreWeb.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
	{
		public ISparePartRepository SparePart { get; }

		public ICategoryRepository Category { get; }

		public IClientRepository Client { get; }

		public UnitOfWork(ISparePartRepository sparePart, ICategoryRepository category, IClientRepository client)
		{
			SparePart = sparePart;
			Category = category;
			Client = client;
		}
	}
}
