using SPSMobile.Data.ViewModels;

namespace SPSMobile.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = serviceProvider.GetRequiredService<SparePartsViewModel>();
	}
}
