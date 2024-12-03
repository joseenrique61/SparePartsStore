using SPSModels.Models;

namespace SPSMobile.Data.Repositories.CategoryRepository
{
	public interface ICategoryRepository
	{
		public List<Category>? GetAll();

		public Category? GetById(int id);

		public Category? GetByName(string name);

		public bool Create(Category category);

		public bool Update(Category category);

		public bool Delete(int id);
	}
}
