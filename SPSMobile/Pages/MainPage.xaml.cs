using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using System.Collections.ObjectModel;

namespace SPSMobile.Pages;

public partial class MainPage : ContentPage
{
    private readonly IUnitOfWork _unitOfOfWork;
    public MainPage(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
        _unitOfOfWork = unitOfWork;
        
        var mainViewModel = new MainViewModel(_unitOfOfWork);
        //BindingContext = new MainViewModel(_unitOfOfWork);

        carouselItems.ItemsSource = mainViewModel.Images;
        collectionItems.ItemsSource = mainViewModel.SpareParts;
    }
}