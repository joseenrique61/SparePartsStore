using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class ProductPage : ContentPage
{
	SparePart SparePart { get; set; }
	public ProductPage(SparePart sparePart)
	{
		InitializeComponent();
		SparePart = sparePart;
		BindingContext = SparePart;
	}
}