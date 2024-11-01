using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPSModels.Models
{
	public class PurchaseOrder
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[ForeignKey(nameof(Client))]
		public int ClientId { get; set; }

		public Client? Client { get; set; }

		public ICollection<Order> Orders { get; set; }

		public bool PurchaseCompleted { get; set; }
	}
}
