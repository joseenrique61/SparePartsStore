using SPSModels.Models;
using System.Collections.ObjectModel;

namespace SPSMobile.Data.ViewModels
{
	public class PurchaseOrderViewModel
	{
		public ObservableCollection<SparePart> SparePartsInCart { get; set; }
		public Client Client { get; set; }
		public User User { get; set; }
		public ICollection<Order> Orders { get; set; }
		public PurchaseOrder PurchaseOrder { get; set; }
	}
}
