using SPSMobile.Data.ViewModels;

namespace SPSMobile.Pages;

public partial class ClientPage : ContentPage
{
	public ClientPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = serviceProvider.GetRequiredService<ClientViewModel>();
	}
}