using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSModels.Models;
using System.Collections.ObjectModel;

namespace SPSMobile.Pages;

public partial class SparepartsPage : ContentPage
{
    private readonly IUnitOfWork _unitOfOfWork;
    private readonly IServiceProvider _serviceProvider;
    private SparePartsViewModel _sparePartsViewModel;

    public SparepartsPage(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
	{
        _unitOfOfWork = unitOfWork;
        _serviceProvider = serviceProvider;

        InitializeComponent();
        LoadDependencies();
    }

    public SparepartsPage(Category selectedCategory)
    {
        InitializeComponent();
        LoadDependencies(selectedCategory);
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

        CategoryPicker.SelectedIndex = 0;
    }

    public void LoadDependencies(Category selectedCategory)
    {
        _sparePartsViewModel = new SparePartsViewModel();
        /*
         {
            SpareParts = new ObservableCollection<SparePart>((await _unitOfOfWork.SparePart.GetAll())!),
            Categories = new ObservableCollection<Category>((await _unitOfOfWork.Category.GetAll())!)
        };*/

        BindingContext = _sparePartsViewModel;

        CategoryPicker.SelectedIndex = selectedCategory.Id;
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

        IEnumerable<SparePart> FilteredSpareParts = new ObservableCollection<SparePart>();

        if (selectedCategory != null && selectedCategory.Name != "All")       
            FilteredSpareParts = _sparePartsViewModel.SpareParts.
                Where(sp => sp.CategoryId == selectedCategory.Id).
                AsEnumerable();
        else       
            FilteredSpareParts = _sparePartsViewModel.SpareParts;

        if (!FilteredSpareParts.Any())
        {
            DisplayAlert("Alert", "I'm sorry, there are not products in the selected category", "Ok");
            sparePartsViewModel.SpareParts = _sparePartsViewModel.SpareParts;
            CategoryPicker.SelectedItem = null;
        }
        else
        {
            sparePartsViewModel.SpareParts = new ObservableCollection<SparePart>(FilteredSpareParts);
        }

        BindingContext = sparePartsViewModel;
    }

    /*
     private void More_Details(object sender, EventArgs e)
    {
        if (BindingContext is SPSModels.Models.SparePart sparepart)
        {
            Navigation.PushModalAsync(_serviceProvider.GetRequiredService<ProductPage>());
        }
    }*/
}