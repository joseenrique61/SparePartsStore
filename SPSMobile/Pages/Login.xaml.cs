using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSMobile.Utilities.Authenticator;

namespace SPSMobile.Pages;

public partial class Login : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;

	private readonly IServiceProvider _serviceProvider;

	private readonly IAuthenticator _authenticator;
	
	public Login(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IAuthenticator authenticator)
	{
		InitializeComponent();

		_unitOfWork = unitOfWork;
		_serviceProvider = serviceProvider;
		_authenticator = authenticator;
	}

	private async void Login_Clicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtinputEmail.Text) || string.IsNullOrEmpty(txtinputPassword.Text))
		{
			await DisplayAlert("Error", "You must fill all fields.", "OK");
			return;
		}
		if (!_unitOfWork.Client.Login(txtinputEmail.Text, txtinputPassword.Text))
		{
			await DisplayAlert("Error", "Invalid Email or password.", "OK");
			return;
		}

		_serviceProvider.GetRequiredService<ClientViewModel>().ClientInfo = _unitOfWork.Client.GetById(_authenticator.ClientInfo.ClientId);

		await Navigation.PopModalAsync();
	}

	private async void Register_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(_serviceProvider.GetRequiredService<Register>());
	}
}