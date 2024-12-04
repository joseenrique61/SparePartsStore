using SPSMobile.Pages;

namespace SPSMobile
{
	public partial class App : Application
	{
		public App(IServiceProvider serviceProvider)
		{
			InitializeComponent();

			Current.UserAppTheme = AppTheme.Light;

			MainPage = new AppShell();
		}
	}
}
