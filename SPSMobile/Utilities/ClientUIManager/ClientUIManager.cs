using SPSMobile.Data.ViewModels;

namespace SPSMobile.Utilities.ClientUIManager
{
	internal class ClientUIManager : IClientUIManager
	{
		private readonly IServiceProvider _serviceProvider;

		public ClientUIManager(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public void UpdateProperties()
		{
			_serviceProvider.GetRequiredService<AppShellViewModel>().UpdateProperties();
			_serviceProvider.GetRequiredService<ClientViewModel>().UpdateProperties();
			_serviceProvider.GetRequiredService<PurchaseOrderViewModel>().UpdateProperties();
			_serviceProvider.GetRequiredService<PreviousOrdersViewModel>().UpdateProperties();
		}
	}
}
