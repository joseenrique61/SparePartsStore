using System.Text.Json;

namespace SPSMobile.Data.FileManager
{
	public class FileManager : IFileManager
	{
		private static string basePath => FileSystem.AppDataDirectory;

		public object? ReadFile(string fileName)
		{
			string content = File.ReadAllText(GetFilePath(fileName));
			return JsonSerializer.Deserialize<object?>(content);
		}

		public void SaveFile(string fileName, object value)
		{
			FileStream fileStream = File.Create(GetFilePath(fileName));
			JsonSerializer.Serialize(fileStream, value);
		}

		private static string GetFilePath(string fileName)
		{
			return Path.Combine(basePath, fileName);
		}
	}
}
