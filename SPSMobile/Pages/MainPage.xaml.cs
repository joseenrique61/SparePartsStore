using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using System.Collections.ObjectModel;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class MainPage : ContentPage
{
    private readonly IUnitOfWork _unitOfOfWork;
    private readonly IServiceProvider _serviceProvider;
    private SparePartsViewModel _sparePartsViewModel; 
    public MainPage(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
	{
        _unitOfOfWork = unitOfWork;
        _serviceProvider = serviceProvider;

		InitializeComponent();
        LoadDependencies();
    }

    public void LoadDependencies() 
    {
        _sparePartsViewModel = new SparePartsViewModel();
        /*
         {
            SpareParts = new ObservableCollection<SparePart>((await _unitOfOfWork.SparePart.GetAll())!),
            Categories = new ObservableCollection<Category>((await _unitOfOfWork.Category.GetAll())!)
        };*/

        BindingContext = _sparePartsViewModel;
    }

    private void SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var product = (SparePart)e.CurrentSelection[0];
            Navigation.PushAsync(new ProductPage(product));

            collectionItems.SelectedItem = null;
        }
    }

    private void OnCategorySelected(object sender, EventArgs e)
    {
        var selectedCategory = (Category)CategoryPicker.SelectedItem;
        var sparePartsViewModel = new SparePartsViewModel();
        /*
         {
            SpareParts = new ObservableCollection<SparePart>((await _unitOfOfWork.SparePart.GetAll())!),
            Categories = new ObservableCollection<Category>((await _unitOfOfWork.Category.GetAll())!)
        };*/

        IEnumerable<SparePart> FilteredSpareParts;

        if (selectedCategory != null)
        {
            FilteredSpareParts = _sparePartsViewModel.SpareParts
                .Where(sp => sp.CategoryId == selectedCategory.Id)
                .AsEnumerable();
            /*if (!FilteredSpareParts.Any()) 
            { 
                selectedCategory = null;
                CategoryPicker.SelectedItem = null;
            }*/
        }
        else
        {
            FilteredSpareParts = _sparePartsViewModel.SpareParts;
        }

        if (!FilteredSpareParts.Any())
        {
            sparePartsViewModel.SpareParts = _sparePartsViewModel.SpareParts;
            selectedCategory = null;
            CategoryPicker.SelectedItem = null;
        }

        /*
         var mainViewModelFromApi = (MainViewModel)BindingContext;
        mainViewModelFromApi.SpareParts = new ObservableCollection<SparePart>(FilteredSpareParts);
                OnPropertyChanged(nameof(mainViewModelFromApi.SpareParts));
        */

        /*var mainViewModel = new MainViewModel
        {
            SpareParts = new ObservableCollection<SparePart>(FilteredSpareParts)
        };*/

        BindingContext = sparePartsViewModel;
    }

    /*
      private void More_Details(object sender, EventArgs e)
     {
         var product = (SparePart)e.;
         Navigation.PushAsync(new ProductPage(product));

         collectionItems.SelectedItem = null;
     }*/
}