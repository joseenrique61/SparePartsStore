using SPSMobile.Data.FileManager;
using SPSModels.Models;

namespace SPSMobile.Data.Repositories.CategoryRepository
{
	internal class LocalCategoryRepository : ICategoryRepository, ILocalRepository
	{
		public string FileName { get; init; } = "categories.json";

		private readonly IFileManager _fileManager;

		public LocalCategoryRepository(IFileManager fileManager)
		{
			_fileManager = fileManager;
		}

		public bool Create(Category category)
		{
			List<Category>? categories = (List<Category>?)_fileManager.ReadFile(FileName);
			categories ??= [];

			category.Id = categories.Count != 0 ? categories.Last().Id + 1 : 1;
			categories.Add(category);

			_fileManager.SaveFile(FileName, categories);
			return true;
		}

		public List<Category>? GetAll()
		{
			return (List<Category>?)_fileManager.ReadFile(FileName);
		}

		public Category? GetById(int id)
		{
			List<Category>? categories = (List<Category>?)_fileManager.ReadFile(FileName);
			if (categories == null)
			{
				return null;
			}
			return categories.Find(c => c.Id == id);
		}

		public Category? GetByName(string name)
		{
			List<Category>? categories = (List<Category>?)_fileManager.ReadFile(FileName);
			if (categories == null)
			{
				return null;
			}
			return categories.Find(c => c.Name == name);
		}

		public bool Update(Category category)
		{
			List<Category>? categories = (List<Category>?)_fileManager.ReadFile(FileName);
			if (categories == null)
			{
				return false;
			}
			
			Category? temp = categories.Find(c => c.Id == category.Id);
			if (temp == null)
			{
				return false;
			}

			temp = category;

			_fileManager.SaveFile(FileName, categories);
			return true;
		}

		public bool Delete(int id)
		{
			List<Category>? categories = (List<Category>?)_fileManager.ReadFile(FileName);
			if (categories == null)
			{
				return false;
			}

			Category? temp = categories.Find(c => c.Id == id);
			if (temp == null)
			{
				return false;
			}

			categories.Remove(temp);

			_fileManager.SaveFile(FileName, categories);
			return true;
		}
	}
}
