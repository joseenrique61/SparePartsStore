using System.Windows.Input;

namespace SPSMobile.Data.ViewModels
{
	internal class InformationViewModel
	{
		public ICommand EmailTapCommand => new Command<string>((url) => Launcher.OpenAsync(url));
		public ICommand PhoneTapCommand => new Command<string>((url) => Launcher.OpenAsync(url));
		public ICommand WhatsAppTapCommand => new Command<string>((url) => Launcher.OpenAsync(url));
	}
}
