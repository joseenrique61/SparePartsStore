namespace SPSMobile.Utilities.AlertService
{
	// Solution from ToolmakerSteve. https://stackoverflow.com/questions/72429055/how-to-displayalert-in-a-net-maui-viewmodel
	internal class AlertService : IAlertService
	{
		public Task ShowAlertAsync(string title, string message, string cancel = "OK")
		{
			return Application.Current!.MainPage!.DisplayAlert(title, message, cancel);
		}

		public void ShowAlert(string title, string message, string cancel)
		{
			Application.Current!.MainPage!.Dispatcher.Dispatch(async () => await ShowAlertAsync(title, message, cancel));
		}
	}
}
