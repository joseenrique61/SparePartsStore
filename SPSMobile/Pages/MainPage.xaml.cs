using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using System.Collections.ObjectModel;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class MainPage : ContentPage
{
    private readonly IUnitOfWork _unitOfOfWork;
    private readonly IServiceProvider _serviceProvider;
    private MainViewModel _mainViewModel; 
    public MainPage(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
	{
        _unitOfOfWork = unitOfWork;
        _serviceProvider = serviceProvider;

		InitializeComponent();
        LoadDependencies();
    }

    public async void LoadDependencies() 
    {
        _mainViewModel = new MainViewModel
        {
            SpareParts = new ObservableCollection<SparePart>((await _unitOfOfWork.SparePart.GetAll())!),
            Categories = new ObservableCollection<Category>((await _unitOfOfWork.Category.GetAll())!)
        };

        BindingContext = _mainViewModel;
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

    private async void OnCategorySelected(object sender, EventArgs e)
    {
        var selectedCategory = (Category)CategoryPicker.SelectedItem;
        var mainViewModel = new MainViewModel
        {
            SpareParts = new ObservableCollection<SparePart>((await _unitOfOfWork.SparePart.GetAll())!),
            Categories = new ObservableCollection<Category>((await _unitOfOfWork.Category.GetAll())!)
        };

        //IEnumerable<SparePart> Categories;
        IEnumerable<SparePart> FilteredSpareParts;

        if (selectedCategory != null)
        {
            FilteredSpareParts = _mainViewModel.SpareParts
                .Where(sp => sp.CategoryId == selectedCategory.Id)
                .ToList();
        }
        else
        {
            FilteredSpareParts = _mainViewModel.SpareParts;
        }

        if (!FilteredSpareParts.Any()) 
            mainViewModel.SpareParts = new ObservableCollection<SparePart>(FilteredSpareParts);

        /*
         var mainViewModelFromApi = (MainViewModel)BindingContext;
        mainViewModelFromApi.SpareParts = new ObservableCollection<SparePart>(FilteredSpareParts);
                OnPropertyChanged(nameof(mainViewModelFromApi.SpareParts));
*/

        /*var mainViewModel = new MainViewModel
        {
            SpareParts = new ObservableCollection<SparePart>(FilteredSpareParts)
        };*/

        BindingContext = mainViewModel;
    }

    private void More_Details(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(_serviceProvider.GetRequiredService<ProductPage>());
    }
}