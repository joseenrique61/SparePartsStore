using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSModels.Models;
using System.Collections.ObjectModel;

namespace SPSMobile.Pages;

public partial class SparepartsPage : ContentPage
{
    private readonly IUnitOfWork _unitOfOfWork;
    private readonly IServiceProvider _serviceProvider;

    public SparepartsPage(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
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

    private void spareCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var product = (SparePart)e.CurrentSelection[0];
            Navigation.PushAsync(new ProductPage(product));

            collectionItems.SelectedItem = null;
        }
    }

    private void More_Details(object sender, EventArgs e)
    {
        /*if (BindingContext is SPSModels.Models.SparePart sparepart)
        {
            Navigation.PushModalAsync(_serviceProvider.GetRequiredService<ProductPage>());
        }*/
    }
}