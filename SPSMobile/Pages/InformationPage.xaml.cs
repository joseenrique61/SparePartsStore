using SPSMobile.Data.ViewModels;

namespace SPSMobile.Pages;

public partial class InformationPage : ContentPage
{
	public InformationPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = serviceProvider.GetRequiredService<InformationViewModel>();
	}
}