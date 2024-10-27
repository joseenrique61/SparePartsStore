using System.ComponentModel.DataAnnotations;

namespace SPSModels.Models
{
	public class Client : User
	{
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
