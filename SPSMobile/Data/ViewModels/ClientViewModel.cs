using SPSMobile.Data.UnitOfWork;
using SPSMobile.Pages;
using SPSMobile.Utilities.Authenticator;
using SPSMobile.Utilities.ClientUIManager;
using SPSModels.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SPSMobile.Data.ViewModels
{
	internal class ClientViewModel : INotifyPropertyChanged
	{
		private Client? clientInfo;

		private bool isSignedIn;

		public Client? ClientInfo
		{
			get
			{
				return clientInfo;
			}
			set
			{
				clientInfo = value;
				OnPropertyChanged();

				IsSignedIn = clientInfo != null;
			}
		}

		public bool IsSignedIn
		{
			get
			{
				return isSignedIn;
			}
			set
			{
				isSignedIn = value;
				OnPropertyChanged();

				IsNotSignedIn = !isSignedIn;
			}
		}

		public bool IsNotSignedIn
		{
			get
			{
				return !isSignedIn;
			}
			set
			{
				OnPropertyChanged();
			}
		}

		public ICommand Logout { get; private set; }
		public ICommand Login { get; private set; }
		public ICommand Register { get; private set; }

		private readonly IUnitOfWork _unitOfWork;

		private readonly IAuthenticator _authenticator;

		public event PropertyChangedEventHandler? PropertyChanged;

		public ClientViewModel(IUnitOfWork unitOfWork, IAuthenticator authenticator, IServiceProvider serviceProvider)
		{
			_unitOfWork = unitOfWork;
			_authenticator = authenticator;

			UpdateProperties();

			Logout = new Command(
				() =>
				{
					_unitOfWork.Client.Logout();
					ClientInfo = null;

					serviceProvider.GetRequiredService<IClientUIManager>().UpdateProperties();
				});

			Login = new Command(
				() =>
				{
					Shell.Current.Navigation.PushAsync(serviceProvider.GetRequiredService<Login>());
				});

			Register = new Command(
				() =>
				{
					Shell.Current.Navigation.PushAsync(serviceProvider.GetRequiredService<Register>());
				});
		}

		public void UpdateProperties()
		{
			ClientInfo = _unitOfWork.Client.GetById(_authenticator.ClientInfo.ClientId);
		}

		public void OnPropertyChanged([CallerMemberName] string name = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
