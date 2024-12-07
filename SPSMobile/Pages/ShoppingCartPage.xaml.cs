using SPSMobile.Data.ViewModels;

namespace SPSMobile.Pages;

public partial class ShoppingCartPage : ContentPage
{
	public ShoppingCartPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		BindingContext = serviceProvider.GetRequiredService<PurchaseOrderViewModel>();
	}

	//public void LoadDependencies() 
	//{
	//       purchaseOrder = _unitOfWork.PurchaseOrder.GetCurrentByClientId(_authenticator.ClientInfo.ClientId);
	//       BindingContext = purchaseOrder;
	//   }

	//   private void DeleteSelectedItems(object sender, EventArgs e)
	//   {

	//   }

	//   private void BuyItems(object sender, EventArgs e)
	//   {
	//       DisplayAlert("Alert", "Thanks for buying!", "Ok");
	//   }
}