using SPSMobile.Data.ViewModels;

namespace SPSMobile
{
	public partial class AppShell : Shell
	{
		public AppShell(IServiceProvider serviceProvider)
		{
			InitializeComponent();

			BindingContext = serviceProvider.GetRequiredService<AppShellViewModel>();
		}
		protected override void OnNavigated(ShellNavigatedEventArgs args)
		{
			base.OnNavigated(args);
			title.Text = Current.CurrentPage.Title;
		}
	}
}
