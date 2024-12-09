using SPSMobile.Data.ApiClient;

namespace SPSMobile.Data.ImageManager
{
	internal class ImageManager : IImageManager
	{
		private readonly IApiClient _client;

		private string basePath => FileSystem.CacheDirectory;

		public ImageManager(IApiClient client)
		{
			_client = client;
		}

		public string GetImagePath(string name)
		{
			string directory = Path.Combine(basePath, "Images");
			string newPath = Path.Combine(directory, name);

			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			if (!File.Exists(newPath))
			{
				using FileStream fileStream = new(newPath, FileMode.Create);
				HttpResponseMessage response = _client!.Get<SPSModels.Models.Image>($"getById/{name}");
				fileStream.Write(response.Content.ReadAsByteArrayAsync().Result);
			}

			return newPath;
		}
	}
}
