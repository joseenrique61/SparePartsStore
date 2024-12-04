using SPSMobile.Data.UnitOfWork;
using SPSMobile.Utilities;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class ProductPage : ContentPage
{
	private SparePart _product;
	private int _desiredQuantity;

	private readonly IUnitOfWork _unitOfWork;

	private readonly IAuthenticator _authenticator;

	private readonly IServiceProvider _serviceProvider;

	public ProductPage(SparePart sparePart, IUnitOfWork unitOfWork, IAuthenticator authenticator, IServiceProvider serviceProvider)
	{
		BindingContext = sparePart;
		_product = sparePart;
		_desiredQuantity = 1;

		_unitOfWork = unitOfWork;
		_authenticator = authenticator;

		_serviceProvider = serviceProvider;

		InitializeComponent();
		UpdateQuantityLabel();
		UpdateButtonStates();
	}

	private void OnIncreaseQuantityClicked(object sender, EventArgs e)
	{
		if (_desiredQuantity < _product.Stock)
		{
			_desiredQuantity++;
			UpdateQuantityLabel();
			UpdateButtonStates();
		}
	}

	private void OnDecreaseQuantityClicked(object sender, EventArgs e)
	{
		if (_desiredQuantity > 1)
		{
			_desiredQuantity--;
			UpdateQuantityLabel();
			UpdateButtonStates();
		}
	}

	private void OnAddToCartClicked(object sender, EventArgs e)
	{
		if (_authenticator.ClientInfo.ClientId == 0)
		{
			Navigation.PushModalAsync(_serviceProvider.GetRequiredService<Login>());
			return;
		}
		PurchaseOrder purchaseOrder = _unitOfWork.PurchaseOrder.GetCurrentByClientId(_authenticator.ClientInfo.ClientId);
		Order? order = purchaseOrder.Orders.FirstOrDefault(o => o.SparePart!.Id == _product.Id);
		if (order == null)
		{
			purchaseOrder.Orders.Add(new Order
			{
				Amount = _desiredQuantity,
				SparePartId = _product.Id
			});
		}
		else
		{
			order.Amount += _desiredQuantity;
		}
		_unitOfWork.PurchaseOrder.Update(purchaseOrder);

		DisplayAlert("Cart", $"{_desiredQuantity} units of {_product.Name} added to the cart.", "OK");
	}

	private void UpdateQuantityLabel()
	{
		QuantityLabel.Text = _desiredQuantity.ToString();
	}

	private void UpdateButtonStates()
	{
		DecreaseButton.IsEnabled = _desiredQuantity > 1;
		IncreaseButton.IsEnabled = _desiredQuantity < _product.Stock;
	}
}