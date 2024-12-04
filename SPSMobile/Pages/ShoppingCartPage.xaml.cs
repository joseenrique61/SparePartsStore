using SparePartsStoreWeb.Data.UnitOfWork;
using SPSMobile.Data.Repositories.PurchaseOrderRepository;

namespace SPSMobile.Pages;

public partial class ShoppingCartPage : ContentPage
{
	private PurchaseOrderRepository _repository; 
	public ShoppingCartPage()
	{
		InitializeComponent();
	}

	public void LoadDependencies() 
	{
        _repository = ;
        BindingContext = _repository;
    }


}