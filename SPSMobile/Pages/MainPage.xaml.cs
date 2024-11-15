using SPSMobile.Data.UnitOfWork;

namespace SPSMobile.Pages
{
	public partial class MainPage : ContentPage
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

		private async void OnCounterClicked(object sender, EventArgs e)
		{
			count++;

			if (count == 1)
				CounterBtn.Text = $"Clicked {count} time";
			else
				CounterBtn.Text = $"Clicked {count} times";

			SemanticScreenReader.Announce(CounterBtn.Text);
		}
	}
}
