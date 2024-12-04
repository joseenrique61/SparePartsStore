namespace SPSMobile.Data.FileManager
{
	public interface IFileManager<T>
	{
		public void SaveFile(string fileName, T? value);

		public T? ReadFile(string fileName);
	}
}
