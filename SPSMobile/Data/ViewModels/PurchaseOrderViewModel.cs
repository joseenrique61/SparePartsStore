using SPSMobile.Data.UnitOfWork;
using SPSMobile.Utilities.AlertService;
using SPSMobile.Utilities.Authenticator;
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

		private ObservableCollection<OrderViewModel> orders;

		private PurchaseOrder purchaseOrder;

		private double total;

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
			get => orders;
			set
			{
				orders = value;
				OnPropertyChanged();
			}
		}

		public Client Client { get; set; }

		public double Total
		{
			get => total;
			set
			{
				total = value;
				OnPropertyChanged();
			}
		}

		public ICommand DeleteOrder { get; private set; }

		public ICommand Buy { get; private set; }

		public PurchaseOrderViewModel(IUnitOfWork unitOfWork, IAuthenticator authenticator, IAlertService alertService)
		{
			_unitOfWork = unitOfWork;
			_authenticator = authenticator;

			UpdateProperties();

			DeleteOrder = new Command((sparePartName) =>
			{
				Orders.Remove(Orders.First(o => o.SparePart.Name == (string)sparePartName));
			});

			Buy = new Command(async () =>
			{
				PurchaseOrder.PurchaseCompleted = true;
				unitOfWork.PurchaseOrder.Update(PurchaseOrder);

				PurchaseOrder = unitOfWork.PurchaseOrder.GetCurrentByClientId(authenticator.ClientInfo.ClientId);

				await alertService.ShowAlertAsync("Thanks!", "Your purchase has been completed, thanks for buying!", "OK");

				UpdateProperties();
			}, () => Orders.Count > 0);
		}

		public void UpdateProperties()
		{
			if (!_authenticator.IsSignedIn)
			{
				return;
			}

			Client = _unitOfWork.Client.GetById(_authenticator.ClientInfo.ClientId)!;

			PurchaseOrder = _unitOfWork.PurchaseOrder.GetCurrentByClientId(_authenticator.ClientInfo.ClientId);

			Orders = new ObservableCollection<OrderViewModel>(PurchaseOrder.Orders.Select(o => new OrderViewModel()
			{
				SparePart = o.SparePart!,
				Amount = o.Amount,
				Total = o.SparePart!.Price * o.Amount,
				DeleteOrder = new Command((sparePartName) =>
				{
					PurchaseOrder.Orders.Remove(PurchaseOrder.Orders.First(o => o.SparePart!.Name == (string)sparePartName));
					_unitOfWork.PurchaseOrder.Update(PurchaseOrder);

					Orders.Remove(Orders.First(o => o.SparePart.Name == (string)sparePartName));

					UpdateProperties();
				})
			}));

			Total = Orders.Sum(o => o.Total);
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string name = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
