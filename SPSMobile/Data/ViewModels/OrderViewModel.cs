using SPSModels.Models;
using System.Windows.Input;

namespace SPSMobile.Data.ViewModels
{
	internal class OrderViewModel
	{
		public SparePart SparePart { get; set; }

		public int Amount { get; set; }

		public double Total { get; set; }

		public ICommand DeleteOrder { get; set; }
	}
}
