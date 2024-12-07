using SPSMobile.Data.UnitOfWork;
using SPSMobile.Utilities.Authenticator;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPSMobile.Data.ViewModels
{
	internal class PreviousOrdersViewModel : INotifyPropertyChanged
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IAuthenticator _authenticator;

		private readonly IServiceProvider _serviceProvider;

		private ObservableCollection<PurchaseOrderViewModel> purchaseOrders;

		public ObservableCollection<PurchaseOrderViewModel> PurchaseOrders
		{
			get => purchaseOrders;
			set
			{
				purchaseOrders = value;
				OnPropertyChanged();
			}
		}

		public PreviousOrdersViewModel(IUnitOfWork unitOfWork, IAuthenticator authenticator, IServiceProvider serviceProvider)
		{
			_unitOfWork = unitOfWork;
			_authenticator = authenticator;
			_serviceProvider = serviceProvider;

			UpdateProperties();
		}

		public void UpdateProperties()
		{
			IEnumerable<PurchaseOrderViewModel>? _purchaseOrders = _unitOfWork.PurchaseOrder.GetByClientId(_authenticator.ClientInfo.ClientId)?
				.OrderByDescending(p => p.Id)
				.Where(p => p.PurchaseCompleted)
				.Select(p => new PurchaseOrderViewModel(_serviceProvider.GetRequiredService<IUnitOfWork>(), _serviceProvider.GetRequiredService<IAuthenticator>(), p));
			if (_purchaseOrders != null)
			{
				PurchaseOrders = new(_purchaseOrders);
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string name = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
