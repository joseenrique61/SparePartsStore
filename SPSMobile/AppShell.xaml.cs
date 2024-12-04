namespace SPSMobile
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
		}
		protected override void OnNavigated(ShellNavigatedEventArgs args)
		{
			base.OnNavigated(args);
			title.Text = Current.CurrentPage.Title;
		}
	}
}
