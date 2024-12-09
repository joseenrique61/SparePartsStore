using SPSMobile.Data.ApiClient;
using SPSMobile.Data.ImageManager;
using SPSModels.Models;
using System.Net.Http.Json;

namespace SPSMobile.Data.Repositories.SparePartRepository
{
	internal class SparePartRepository : ISparePartRepository
	{
		private readonly IApiClient _client;

		private readonly IImageManager _imageManager;

		public SparePartRepository(IApiClient client, IImageManager imageManager)
		{
			_client = client;
			_imageManager = imageManager;
		}

		public List<SparePart>? GetAll()
		{
			HttpResponseMessage response = _client.Get<SparePart>("all");
			if (response.IsSuccessStatusCode)
			{
				List<SparePart> spareParts = response.Content.ReadFromJsonAsync<List<SparePart>>().Result!;
				spareParts = SetImages(spareParts);
				return spareParts;
			}
			return null;
		}

		public SparePart? GetById(int id)
		{
			HttpResponseMessage response = _client.Get<SparePart>($"byId/{id}");
			if (response.IsSuccessStatusCode)
			{
				SparePart sparePart = response.Content.ReadFromJsonAsync<SparePart>().Result!;
				sparePart = SetImage(sparePart);
				return sparePart;
			}
			return null;
		}

		public List<SparePart>? GetByCategory(string categoryName)
		{
			HttpResponseMessage response = _client.Get<SparePart>($"byCategoryName/{categoryName}");
			if (response.IsSuccessStatusCode)
			{
				List<SparePart> spareParts = response.Content.ReadFromJsonAsync<List<SparePart>>().Result!;
				spareParts = SetImages(spareParts);
				return spareParts;
			}
			return null;
		}

		private List<SparePart> SetImages(List<SparePart> spareParts)
		{
			foreach (SparePart part in spareParts)
			{
				SetImage(part);
			}
			return spareParts;
		}

		private SparePart SetImage(SparePart sparePart)
		{
			sparePart.Image = _imageManager.GetImagePath(sparePart.Image);
			return sparePart;
		}

		public bool Create(SparePart sparePart)
		{
			HttpResponseMessage response = _client.Post("create", sparePart);
			return response.IsSuccessStatusCode;
		}

		public bool Update(SparePart sparePart)
		{
			HttpResponseMessage response = _client.Put("update", sparePart);
			return response.IsSuccessStatusCode;
		}

		public bool Delete(int id)
		{
			HttpResponseMessage response = _client.Delete<SparePart>($"delete/{id}");
			return response.IsSuccessStatusCode;
		}
	}
}
