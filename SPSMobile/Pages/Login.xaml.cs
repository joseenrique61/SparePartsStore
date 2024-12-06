using SPSMobile.Data.ViewModels;

namespace SPSMobile.Pages;

public partial class Login : ContentPage
{
	public Login(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = serviceProvider.GetRequiredService<LoginViewModel>();
	}

	//private async void Login_Clicked(object sender, EventArgs e)
	//{
	//	if (string.IsNullOrEmpty(txtinputEmail.Text) || string.IsNullOrEmpty(txtinputPassword.Text))
	//	{
	//		await DisplayAlert("Error", "You must fill all fields.", "OK");
	//		return;
	//	}
	//	if (!_unitOfWork.Client.Login(txtinputEmail.Text, txtinputPassword.Text))
	//	{
	//		await DisplayAlert("Error", "Invalid Email or password.", "OK");
	//		return;
	//	}

	//	_serviceProvider.GetRequiredService<ClientViewModel>().UpdateProperties();

	//	await Navigation.PopAsync();
	//}

	//private async void Register_Clicked(object sender, EventArgs e)
	//{
	//	await Navigation.PushAsync(_serviceProvider.GetRequiredService<Register>());
	//}
}