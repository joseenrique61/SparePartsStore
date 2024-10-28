using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPSModels.Models
{
	public class Client
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(User))]
		public int UserId { get; set; }
		
		public User? User { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Required]
		[MaxLength(100)]
		public string Address { get; set; }

		[Required]
		[MaxLength(50)]
		public string City { get; set; }

		[Required]
		[MaxLength(50)]
		public string Country { get; set; }
	}
}
