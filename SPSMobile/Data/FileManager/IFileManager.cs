namespace SPSMobile.Data.FileManager
{
	public interface IFileManager
	{
		public void SaveFile(string fileName, object value);

		public object? ReadFile(string fileName);
	}
}
