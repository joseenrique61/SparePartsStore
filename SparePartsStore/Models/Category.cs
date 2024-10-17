using System.ComponentModel.DataAnnotations;

namespace SparePartsStore.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
	}
}
