using SPSMobile.Data.UnitOfWork;
using SPSModels.Models;
using System.Collections.ObjectModel;

namespace SPSMobile.Pages;

public partial class CategoriesPage : ContentPage
{
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<Category> FilteredCategories { get; set; }
    private readonly IUnitOfWork _unitOfWork;

    public CategoriesPage(IUnitOfWork unitOfWork)
	{
		InitializeComponent();
        _unitOfWork = unitOfWork;

        Categories = new ObservableCollection<Category>();
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        FilterCategories(e.NewTextValue);
    }

    private void FilterCategories(string newTextValue)
    {
        if (string.IsNullOrWhiteSpace(newTextValue))
        {
            FilteredCategories = new ObservableCollection<Category>(Categories);
        }
        else
        {
            FilteredCategories = new ObservableCollection<Category>(
                Categories.Where(c => c.Name.ToLower().Contains(newTextValue.ToLower())));
        }
        OnPropertyChanged(nameof(FilteredCategories));
    }
}