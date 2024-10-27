using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPSModels.Models
{
	public class SparePart
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(30)]
		public string Name { get; set; }

		[Required]
		[MaxLength(30)]
		public string Description { get; set; }

		[Required]
		[Range(0, int.MaxValue)]
		public int Stock { get; set; }

		[Required]
		public string Image { get; set; }

		[ForeignKey(nameof(Category))]
		public int CategoryId { get; set; }

		[Required]
		public Category Category { get; set; }
	}
}
