namespace SparePartsStoreWeb.Data.Repositories
{
	public interface IClientRepository
	{
		public Task<bool> Login(string email, string password);
	}
}
