using System.Text.Json;

namespace SPSMobile.Data.FileManager
{
	public class FileManager<T> : IFileManager<T>
	{
		private static string basePath => FileSystem.AppDataDirectory;

		public T? ReadFile(string fileName)
		{
			try
			{
				if (!File.Exists(GetFilePath(fileName)))
				{
					FileStream fileStream = File.Create(GetFilePath(fileName));
					fileStream.Close();
				}
				string content = File.ReadAllText(GetFilePath(fileName));
				return string.IsNullOrEmpty(content) ? default : JsonSerializer.Deserialize<T?>(content);
			}
			catch
			{
				return default;
			}
		}

		public void SaveFile(string fileName, T? value)
		{
			FileStream fileStream = new(GetFilePath(fileName), FileMode.Create);
			JsonSerializer.Serialize(fileStream, value);
			fileStream.Close();
		}

		private static string GetFilePath(string fileName)
		{
			return Path.Combine(basePath, fileName);
		}
	}
}
