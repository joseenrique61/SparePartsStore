using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using System.Collections.ObjectModel;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class MainPage : ContentPage
{
    private readonly IUnitOfWork _unitOfOfWork;
    private readonly IServiceProvider _serviceProvider;
    public MainPage(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
	{
        _unitOfOfWork = unitOfWork;
        _serviceProvider = serviceProvider;

		InitializeComponent();
        LoadDependencies();
    }

    public async void LoadDependencies() 
    {
        var mainViewModel = new MainViewModel
        {
            SpareParts = new ObservableCollection<SparePart>((await _unitOfOfWork.SparePart.GetAll())!)
        };

        BindingContext = mainViewModel;
    }

    private void More_Details(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(_serviceProvider.GetRequiredService<ProductPage>());
    }
}