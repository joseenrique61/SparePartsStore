using SPSMobile.Data.ViewModels;

namespace SPSMobile.Pages;

public partial class Login : ContentPage
{
	public Login(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = serviceProvider.GetRequiredService<LoginViewModel>();
	}
}