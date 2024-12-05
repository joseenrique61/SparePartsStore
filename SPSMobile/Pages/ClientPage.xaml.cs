using SPSMobile.Data.UnitOfWork;
using SPSMobile.Utilities.Authenticator;
using SPSModels.Models;

namespace SPSMobile.Pages;

public partial class ClientPage : ContentPage
{
    private Client client;
    private readonly IAuthenticator _authenticator;
    private readonly IUnitOfWork _unitOfWork;

    public ClientPage(IUnitOfWork unitOfWork, IAuthenticator authenticator)
	{
        _unitOfWork = unitOfWork;
        _authenticator = authenticator;
		InitializeComponent();
        LoadDependencies();
	}

    public void LoadDependencies() 
    {
        client = _unitOfWork.Client.GetById(_authenticator.ClientInfo.ClientId);
        BindingContext = client;
    }

    private void LogOutClicked(object sender, EventArgs e)
    {
        _unitOfWork.Client.Logout();
    }
}