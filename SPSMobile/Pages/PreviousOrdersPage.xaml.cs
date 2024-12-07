using SPSMobile.Data.ViewModels;

namespace SPSMobile.Pages;

public partial class PreviousOrdersPage : ContentPage
{
	public PreviousOrdersPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = serviceProvider.GetRequiredService<PreviousOrdersViewModel>();
	}
}