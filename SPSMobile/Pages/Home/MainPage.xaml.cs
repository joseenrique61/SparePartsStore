using SPSMobile.Data.UnitOfWork;
using System.Collections.ObjectModel;

namespace SPSMobile.Pages.Home;

public partial class MainPage : ContentPage
{
    //private readonly IUnitOfWork _unitOfOfWork;
    public MainPage()
	{
		InitializeComponent();
        //_unitOfOfWork = unitOfWork;
        //BindingContext = new MainViewModel(_unitOfOfWork);
        BindingContext = new MainViewModel();
    }
}