namespace SPSMobile.Utilities.AlertService
{
	internal interface IAlertService
	{
		public Task ShowAlertAsync(string title, string message, string cancel);
		
		public void ShowAlert(string title, string message, string cancel);
	}
}
