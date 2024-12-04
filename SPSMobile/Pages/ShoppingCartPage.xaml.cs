using SparePartsStoreWeb.Data.UnitOfWork;
using SPSMobile.Data.Repositories.PurchaseOrderRepository;
using SPSMobile.Data.UnitOfWork;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class ShoppingCartPage : ContentPage
{
	private PurchaseOrder purchaseOrder;
    private UnitOfWork _unitOfWork; 
	public ShoppingCartPage()
	{
		InitializeComponent();
	}

	public async void LoadDependencies(UnitOfWork unitOfWork) 
	{
        /*_unitOfWork = unitOfWork;
		purchaseOrder = await _unitOfWork.PurchaseOrder.GetCurrentByClientId(1);
        BindingContext = purchaseOrder;*/
    }


}