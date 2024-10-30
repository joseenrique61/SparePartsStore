using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories
{
	public interface ISparePartRepository
	{
		public Task<List<SparePart>?> GetAll();

		public Task Create(SparePart sparePart);
	}
}
