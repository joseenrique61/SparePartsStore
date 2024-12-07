using SPSMobile.Data.UnitOfWork;
using SPSMobile.Pages;
using SPSMobile.Utilities.AlertService;
using SPSMobile.Utilities.Authenticator;
using SPSModels.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SPSMobile.Data.ViewModels
{
	internal class ProductViewModel : INotifyPropertyChanged
	{
		private int desiredAmount;

		public SparePart SparePart { get; set; }

		public int DesiredAmount
		{
			get => desiredAmount;
			set
			{
				desiredAmount = value;
				OnPropertyChanged();
			}
		}

		public ICommand IncreaseAmount { get; private set; }

		public ICommand DecreaseAmount { get; private set; }

		public ICommand AddToCart { get; private set; }

		public ProductViewModel(SparePart sparePart, IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IAuthenticator authenticator, IAlertService alertService)
		{
			SparePart = sparePart;

			IncreaseAmount = new Command(() =>
			{
				DesiredAmount += 1;
				UpdateCommands();
			}, () => DesiredAmount < SparePart.Stock);

			DecreaseAmount = new Command(() =>
			{
				DesiredAmount -= 1;
				UpdateCommands();
			}, () => DesiredAmount > 0);

			AddToCart = new Command(async () =>
			{
				if (authenticator.ClientInfo.ClientId == 0)
				{
					await Shell.Current.Navigation.PushAsync(serviceProvider.GetRequiredService<Login>());
					return;
				}
				PurchaseOrder purchaseOrder = unitOfWork.PurchaseOrder.GetCurrentByClientId(authenticator.ClientInfo.ClientId);
				Order? order = purchaseOrder.Orders.FirstOrDefault(o => o.SparePartId == SparePart.Id);
				if (order == null)
				{
					purchaseOrder.Orders.Add(new Order
					{
						Amount = DesiredAmount,
						SparePartId = SparePart.Id
					});
				}
				else
				{
					order.Amount += DesiredAmount;
				}
				unitOfWork.PurchaseOrder.Update(purchaseOrder);

				await alertService.ShowAlertAsync("Cart", $"{DesiredAmount} units of {SparePart.Name} added to the cart.", "OK");

				serviceProvider.GetRequiredService<PurchaseOrderViewModel>().UpdateProperties();
			}, () => DesiredAmount > 0);
		}

		private void UpdateCommands()
		{
			(IncreaseAmount as Command)!.ChangeCanExecute();
			(DecreaseAmount as Command)!.ChangeCanExecute();
			(AddToCart as Command)!.ChangeCanExecute();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string name = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
