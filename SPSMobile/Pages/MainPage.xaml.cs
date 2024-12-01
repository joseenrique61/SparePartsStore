using SPSMobile.Data.UnitOfWork;
using System.Collections.ObjectModel;

namespace SPSMobile.Pages;

public partial class MainPage : ContentPage
{
    private readonly IUnitOfWork _unitOfOfWork;
    public MainPage(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
        _unitOfOfWork = unitOfWork;
        BindingContext = new MainViewModel(_unitOfOfWork);
    }
}