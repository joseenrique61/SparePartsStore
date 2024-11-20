using SPSMobile.Data.UnitOfWork;

namespace SPSMobile.Pages
{
	public partial class MainPage : FlyoutPage
    {
		int count = 0;

		private readonly IUnitOfWork _unitOfWork;

		private readonly IServiceProvider _serviceProvider;

        public MainPage(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ClientPage());
        }
    }
}
