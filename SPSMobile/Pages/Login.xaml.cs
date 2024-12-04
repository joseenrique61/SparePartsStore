using SPSMobile.Data.UnitOfWork;

namespace SPSMobile.Pages;

public partial class Login : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;

	private readonly IServiceProvider _serviceProvider;
	
	public Login(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
	{
		InitializeComponent();

		_unitOfWork = unitOfWork;
		_serviceProvider = serviceProvider;
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

		await Navigation.PopModalAsync();
	}

	private async void Register_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(_serviceProvider.GetRequiredService<Register>());
	}
}