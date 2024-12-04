using SPSMobile.Data.UnitOfWork;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class Register : ContentPage
{
	private readonly IUnitOfWork _unitOfWork;

	public Register(IUnitOfWork unitOfWork)
	{
		InitializeComponent();

		_unitOfWork = unitOfWork;
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		Client client = new()
		{
			User = new()
			{
				Email = txtinputEmail.Text,
				Password = txtinputPassword.Text
			},
			Name = txtinputName.Text,
			Address = txtinputAddress.Text,
			City = txtinputCity.Text,
			Country = txtinputCountry.Text
		};

		if (string.IsNullOrEmpty(txtinputEmail.Text) && string.IsNullOrEmpty(txtinputPassword.Text) && string.IsNullOrEmpty(txtinputName.Text) && string.IsNullOrEmpty(txtinputAddress.Text) && string.IsNullOrEmpty(txtinputCity.Text) && string.IsNullOrEmpty(txtinputCountry.Text))
		{
			await DisplayAlert("Error", "You must fill all the fields.", "OK");
			return;
		}

		if(!_unitOfWork.Client.Register(client))
		{
			await DisplayAlert("Error", "This Email is already registered.", "OK");
			return;
		}

		await Navigation.PopModalAsync();
	}
}