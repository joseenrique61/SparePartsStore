using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPSModels.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[ForeignKey(nameof(PurchaseOrder))]
		public int PurchaseOrderId {  get; set; }

		public PurchaseOrder? PurchaseOrder { get; set; }

		[Required]
		[ForeignKey(nameof(SparePart))]
		public int SparePartId { get; set; }
		
		public SparePart? SparePart {  get; set; }
	}
}
