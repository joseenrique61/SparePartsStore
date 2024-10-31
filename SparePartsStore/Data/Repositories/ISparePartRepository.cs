using SPSModels.Models;

namespace SparePartsStoreWeb.Data.Repositories
{
	public interface ISparePartRepository
	{
		public Task<List<SparePart>?> GetAll();

		public Task<SparePart?> GetById(int id);

		public Task<SparePart?> GetByCategory(string categoryName);

		public Task<bool> Create(SparePart sparePart);

		public Task<bool> Update(SparePart sparePart);
		
		public Task<bool> Delete(int id);
	}
}
