using SPSMobile.Pages;

namespace SPSMobile
{
	public partial class App : Application
	{
		public App(IServiceProvider serviceProvider)
		{
			InitializeComponent();

			MainPage = new NavigationPage(serviceProvider.GetRequiredService<MainPage>());
		}
	}
}
