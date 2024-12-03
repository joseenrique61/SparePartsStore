using SPSMobile.Data.ViewModels;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class CategoriesPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private SparePartsViewModel _sparePartsViewModel;
	public CategoriesPage()
	{
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
            var category = (Category)e.CurrentSelection[0];
            Navigation.PushAsync(new SparepartsPage(category));

            collectionItems.SelectedItem = null;
        }
    }
}