using SPSModels.Models;

namespace SPSMobile.Data.Repositories.SparePartRepository
{
	public interface ISparePartRepository
	{
		public List<SparePart>? GetAll();

		public SparePart? GetById(int id);

		public List<SparePart>? GetByCategory(string categoryName);

		public bool Create(SparePart sparePart);

		public bool Update(SparePart sparePart);

		public bool Delete(int id);
	}
}
