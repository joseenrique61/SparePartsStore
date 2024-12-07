using SPSMobile.Utilities.Authenticator;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPSMobile.Data.ViewModels
{
	internal class AppShellViewModel : INotifyPropertyChanged
	{
		private readonly IAuthenticator _authenticator;

		public event PropertyChangedEventHandler? PropertyChanged;

		private bool cartVisible;

		public bool CartVisible
		{
			get => cartVisible;
			set
			{
				cartVisible = value;
				OnPropertyChanged();
			}
		}

		public AppShellViewModel(IAuthenticator authenticator)
		{
			_authenticator = authenticator;

			UpdateProperties();
		}

		public void UpdateProperties()
		{
			CartVisible = _authenticator.IsSignedIn;
		}

		public void OnPropertyChanged([CallerMemberName] string name = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
