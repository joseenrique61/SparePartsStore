using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class ProductPage : ContentPage
{
    //public SparePart SparePart { get; set; }
    private SparePart _product;
    private int _desiredQuantity;

    public ProductPage(SparePart sparePart)
	{
		BindingContext = sparePart;
        _product = sparePart;
        _desiredQuantity = 1;

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
        // Lógica para añadir al carrito
        DisplayAlert("Carrito", $"{_desiredQuantity} unidad(es) de {_product.Name} añadida(s) al carrito.", "OK");
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