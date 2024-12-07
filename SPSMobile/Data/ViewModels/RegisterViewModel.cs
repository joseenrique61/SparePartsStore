using SPSMobile.Data.UnitOfWork;
using SPSMobile.Utilities.AlertService;
using SPSMobile.Utilities.ClientUIManager;
using SPSModels.Models;
using System.Windows.Input;

namespace SPSMobile.Data.ViewModels
{
	internal class RegisterViewModel
	{
		public Client Client { get; set; } = new();

		public ICommand Register { get; private set; }

		public RegisterViewModel(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IAlertService alertService)
		{
			Client.User = new();

			Register = new Command(async () =>
			{
				if (string.IsNullOrEmpty(Client.User.Email) || string.IsNullOrEmpty(Client.User.Password) || string.IsNullOrEmpty(Client.Name) || string.IsNullOrEmpty(Client.Address) || string.IsNullOrEmpty(Client.City) || string.IsNullOrEmpty(Client.Country))
				{
					await alertService.ShowAlertAsync("Error", "You must fill all the fields.", "OK");
					return;
				}

				if (!unitOfWork.Client.Register(Client))
				{
					await alertService.ShowAlertAsync("Error", "This Email is already registered.", "OK");
					return;
				}

				serviceProvider.GetRequiredService<IClientUIManager>().UpdateProperties();

				await Shell.Current.Navigation.PopToRootAsync();
			});
		}
	}
}
