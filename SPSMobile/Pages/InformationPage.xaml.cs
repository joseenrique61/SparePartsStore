using System.Windows.Input;

namespace SPSMobile.Pages;

public partial class InformationPage : ContentPage
{
    public ICommand EmailTapCommand => new Command<string>((url) => Launcher.OpenAsync(url));
    public ICommand PhoneTapCommand => new Command<string>((url) => Launcher.OpenAsync(url));
    public ICommand WhatsAppTapCommand => new Command<string>((url) => Launcher.OpenAsync(url));

    public InformationPage()
	{
		InitializeComponent();
	}


}