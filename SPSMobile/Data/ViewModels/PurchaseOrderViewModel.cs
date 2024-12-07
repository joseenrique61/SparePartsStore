using SPSMobile.Data.UnitOfWork;
using SPSMobile.Utilities.AlertService;
using SPSMobile.Utilities.Authenticator;
using SPSMobile.Utilities.ClientUIManager;
using SPSModels.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SPSMobile.Data.ViewModels
{
	internal class PurchaseOrderViewModel : INotifyPropertyChanged
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IAuthenticator _authenticator;

		private PurchaseOrder purchaseOrder;

		public PurchaseOrder PurchaseOrder
		{
			get => purchaseOrder;
			set
			{
				purchaseOrder = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<OrderViewModel> Orders
		{
			get => new(PurchaseOrder.Orders.Select(o => new OrderViewModel()
			{
				SparePart = o.SparePart!,
				Amount = o.Amount,
				Total = o.SparePart!.Price * o.Amount,
				DeleteOrder = new Command<string>((sparePartName) =>
				{
					PurchaseOrder.Orders.Remove(PurchaseOrder.Orders.First(o => o.SparePart!.Name == sparePartName));
					_unitOfWork.PurchaseOrder.Update(PurchaseOrder);

					UpdateProperties(true);
				})
			}));
		}

		public Client Client { get; set; }

		public double Total
		{
			get => Orders.Sum(o => o.Total);
		}

		public ICommand Buy { get; private set; }

		public PurchaseOrderViewModel(IUnitOfWork unitOfWork, IAuthenticator authenticator, PurchaseOrder? purchaseOrder = null, IClientUIManager? clientUIManager = null, IAlertService? alertService = null)
		{
			_unitOfWork = unitOfWork;
			_authenticator = authenticator;

			if (purchaseOrder != null)
			{
				PurchaseOrder = purchaseOrder;
				UpdateProperties(false);
				return;
			}

			UpdateProperties(true);

			Buy = new Command(async () =>
			{
				PurchaseOrder.PurchaseCompleted = true;
				unitOfWork.PurchaseOrder.Update(PurchaseOrder);

				PurchaseOrder = unitOfWork.PurchaseOrder.GetCurrentByClientId(authenticator.ClientInfo.ClientId);

				await alertService!.ShowAlertAsync("Thanks!", "Your purchase has been completed, thanks for buying!", "OK");

				UpdateProperties(true);
				clientUIManager!.UpdateProperties();
			}, () => Orders.Count > 0);

			(Buy as Command)!.ChangeCanExecute();
		}

		public void UpdateProperties(bool currentPurchaseOrder = true)
		{
			if (!_authenticator.IsSignedIn)
			{
				return;
			}

			Client = _unitOfWork.Client.GetById(_authenticator.ClientInfo.ClientId)!;

			if (currentPurchaseOrder)
			{
				PurchaseOrder = _unitOfWork.PurchaseOrder.GetCurrentByClientId(_authenticator.ClientInfo.ClientId);
			}

			OnPropertyChanged(nameof(Orders));
			OnPropertyChanged(nameof(Total));
			(Buy as Command)?.ChangeCanExecute();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string name = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
