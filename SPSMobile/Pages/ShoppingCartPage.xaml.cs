using SparePartsStoreWeb.Data.UnitOfWork;
using SPSMobile.Data.Repositories.PurchaseOrderRepository;
using SPSMobile.Data.UnitOfWork;
using SPSMobile.Utilities.Authenticator;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class ShoppingCartPage : ContentPage
{
	private readonly IAuthenticator _authenticator;
    private readonly IUnitOfWork _unitOfWork; 
	private ClientViewModel _clientViewModel;
    private PurchaseOrder purchaseOrder;
	public ShoppingCartPage(IUnitOfWork unitOfWork, IAuthenticator authenticator)
	{
        _unitOfWork = unitOfWork;
        _authenticator = authenticator;
		InitializeComponent();
        LoadDependencies();
	}

	public void LoadDependencies() 
	{
        purchaseOrder = _unitOfWork.PurchaseOrder.GetCurrentByClientId(_authenticator.ClientInfo.ClientId);
        BindingContext = purchaseOrder;
    }

    private void DeleteSelectedItems(object sender, EventArgs e)
    {

    }

    private void BuyItems(object sender, EventArgs e)
    {
        DisplayAlert("Alert", "Thanks for buying!", "Ok");
    }
}