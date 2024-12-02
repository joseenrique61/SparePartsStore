using SPSMobile.Data.UnitOfWork;

namespace SPSMobile.Pages;

public partial class SparepartsPage : ContentPage
{
    private readonly IUnitOfWork _unitOfOfWork;

    public SparepartsPage(IUnitOfWork unitOfWork)
	{
        _unitOfOfWork = unitOfWork;

        InitializeComponent();
        LoadDependencies();
    }

    public async void LoadDependencies()
    {
        collectionItems.ItemsSource = await _unitOfOfWork.SparePart.GetAll();
    }
}