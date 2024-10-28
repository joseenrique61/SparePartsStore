using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SPSModels.Models
{
	[Index(nameof(Name), IsUnique = true)]
	public class Category
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
	}
}
