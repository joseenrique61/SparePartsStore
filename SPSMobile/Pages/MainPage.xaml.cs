using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using System.Collections.ObjectModel;

namespace SPSMobile.Pages;

public partial class MainPage : ContentPage
{
    private readonly IUnitOfWork _unitOfOfWork;
    public MainPage(IUnitOfWork unitOfWork)
	{
        _unitOfOfWork = unitOfWork;

		InitializeComponent();
        LoadDependencies();
    }

    public async void LoadDependencies() 
    {
        var mainViewModel = new MainViewModel();

        carouselItems.ItemsSource = mainViewModel.Images;
        collectionItems.ItemsSource = await _unitOfOfWork.SparePart.GetAll();
    }
}