using SPSMobile.Data.UnitOfWork;
using SPSMobile.Pages;
using SPSMobile.Utilities.AlertService;
using SPSModels.Models;
using System.Windows.Input;

namespace SPSMobile.Data.ViewModels
{
	internal class LoginViewModel
	{
		public ICommand Login { get; private set; }

		public ICommand Register { get; private set; }

		public User User { get; set; } = new();

		public LoginViewModel(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IAlertService alertService)
		{
			Login = new Command(async () =>
			{
				if (string.IsNullOrEmpty(User!.Email) || string.IsNullOrEmpty(User!.Password))
				{
					await alertService.ShowAlertAsync("Error", "You must fill all fields.", "OK");
					return;
				}
				if (!unitOfWork.Client.Login(User.Email, User.Password))
				{
					await alertService.ShowAlertAsync("Error", "Invalid Email or password.", "OK");
					return;
				}

				serviceProvider.GetRequiredService<ClientViewModel>().UpdateProperties();

				await Shell.Current.Navigation.PopToRootAsync();
			});

			Register = new Command(async () => await Shell.Current.Navigation.PushAsync(serviceProvider.GetRequiredService<Register>()));
		}
	}
}
