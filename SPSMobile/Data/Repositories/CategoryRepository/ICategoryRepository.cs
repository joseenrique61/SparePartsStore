using SPSModels.Models;

namespace SPSMobile.Data.Repositories.CategoryRepository
{
	public interface ICategoryRepository
	{
		public Task<List<Category>?> GetAll();

		public Task<Category?> GetById(int id);

		public Task<Category?> GetByName(string name);

		public Task<bool> Create(Category category);

		public Task<bool> Update(Category category);

		public Task<bool> Delete(int id);
	}
}
