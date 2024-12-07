using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSMobile.Utilities.AlertService;
using SPSMobile.Utilities.Authenticator;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class ProductPage : ContentPage
{
	public ProductPage(SparePart sparePart, IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = new ProductViewModel(sparePart, serviceProvider.GetRequiredService<IUnitOfWork>(), serviceProvider, serviceProvider.GetRequiredService<IAuthenticator>(), serviceProvider.GetRequiredService<IAlertService>());
	}
}