using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class ProductPage : ContentPage
{
	//public SparePart SparePart { get; set; }
	public ProductPage(SparePart sparePart)
	{
		InitializeComponent();
		BindingContext = sparePart;
	}


}