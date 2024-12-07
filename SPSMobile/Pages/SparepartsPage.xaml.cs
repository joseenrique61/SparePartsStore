using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSMobile.Utilities.Authenticator;
using SPSModels.Models;
using System.Collections.ObjectModel;

namespace SPSMobile.Pages;

public partial class SparepartsPage : ContentPage
{
    public SparepartsPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        BindingContext = serviceProvider.GetRequiredService<SparePartsViewModel>();
    }
}