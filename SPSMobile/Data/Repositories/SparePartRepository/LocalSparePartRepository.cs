using SPSMobile.Data.FileManager;
using SPSMobile.Data.Repositories.CategoryRepository;
using SPSModels.Models;

namespace SPSMobile.Data.Repositories.SparePartRepository
{
	internal class LocalSparePartRepository : ISparePartRepository, ILocalRepository
	{
		public string FileName { get; init; } = "spareParts.json";

		private readonly FileManager<List<SparePart>> _fileManager = new();

		private readonly ICategoryRepository _categoryRepository;

		public LocalSparePartRepository(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public List<SparePart>? GetAll()
		{
			return (List<SparePart>?)_fileManager.ReadFile(FileName);
		}

		public List<SparePart>? GetByCategory(string categoryName)
		{
			List<SparePart>? spareParts = (List<SparePart>?)_fileManager.ReadFile(FileName);
			if (spareParts == null)
			{
				return null;
			}

			spareParts.ForEach(s => s.Category = _categoryRepository.GetById(s.CategoryId));

			return spareParts.Where(c => c.Category!.Name == categoryName).ToList();
		}

		public SparePart? GetById(int id)
		{
			List<SparePart>? spareParts = (List<SparePart>?)_fileManager.ReadFile(FileName);
			if (spareParts == null)
			{
				return null;
			}
			return spareParts.Find(c => c.Id == id);
		}

		public bool Create(SparePart sparePart)
		{
			List<SparePart>? spareParts = (List<SparePart>?)_fileManager.ReadFile(FileName);
			spareParts ??= [];

			sparePart.Id = spareParts.Count != 0 ? spareParts.Last().Id + 1 : 1;
			sparePart.Category = null;
			
			//Image logic
			spareParts.Add(sparePart);

			_fileManager.SaveFile(FileName, spareParts);
			return true;
		}

		public bool Update(SparePart sparePart)
		{
			List<SparePart>? spareParts = (List<SparePart>?)_fileManager.ReadFile(FileName);
			if (spareParts == null)
			{
				return false;
			}

			SparePart? temp = spareParts.Find(c => c.Id == sparePart.Id);
			if (temp == null)
			{
				return false;
			}

			sparePart.Category = null;

			int index = spareParts.IndexOf(temp);
			spareParts.Remove(temp);
			spareParts.Insert(index, sparePart);

			//Image logic

			_fileManager.SaveFile(FileName, spareParts);
			return true;
		}

		public bool Delete(int id)
		{
			throw new NotImplementedException();
		}
	}
}
