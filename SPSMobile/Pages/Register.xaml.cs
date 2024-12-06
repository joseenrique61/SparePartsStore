using SPSMobile.Data.ViewModels;

namespace SPSMobile.Pages;

public partial class Register : ContentPage
{
	public Register(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = serviceProvider.GetRequiredService<RegisterViewModel>();
	}
}