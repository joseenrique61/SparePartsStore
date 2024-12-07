using SPSMobile.Data.UnitOfWork;
using SPSMobile.Pages;
using SPSModels.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPSMobile.Data.ViewModels;
public class SparePartsViewModel : INotifyPropertyChanged
{
	private readonly IServiceProvider _serviceProvider;

	private SparePart selectedSparePart;

	private Category selectedCategory;

	private ObservableCollection<SparePart> filteredSpareParts;

	public ObservableCollection<string> Images { get; set; } =
	[
		"freno.jpeg",
		"motor.jpeg",
		"radiador.webp"
	];

	public ObservableCollection<SparePart> SpareParts { get; set; }

	public ObservableCollection<Category> Categories { get; set; }

	public ObservableCollection<SparePart> FilteredSpareParts
	{
		get => filteredSpareParts;
		set
		{
			filteredSpareParts = value;
			OnPropertyChanged();
		}
	}

	public SparePart SelectedSparePart
	{
		get => selectedSparePart;
		set
		{
			selectedSparePart = value;
			SparePartSelected();
		}
	}

	public Category SelectedCategory
	{
		get => selectedCategory;
		set
		{
			selectedCategory = value;

			if (selectedCategory.Name == "All")
			{
				FilteredSpareParts = SpareParts;
			}
			else
			{
				FilteredSpareParts = new ObservableCollection<SparePart>(SpareParts.Where(sp => sp.CategoryId == selectedCategory.Id));
			}
		}
	}

	public SparePartsViewModel(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;

		SpareParts = new ObservableCollection<SparePart>(unitOfWork.SparePart.GetAll()!);
		Categories = new ObservableCollection<Category>(unitOfWork.Category.GetAll()!.Prepend(new Category
		{
			Name = "All"
		}));

		SelectedCategory = Categories.First();
	}

	private async void SparePartSelected()
	{
		await Shell.Current.Navigation.PushAsync(new ProductPage(SelectedSparePart, _serviceProvider));
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	public void OnPropertyChanged([CallerMemberName] string name = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}