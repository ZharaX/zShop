using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
	public class Customer
	{
		public int CustomerID { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(2), MaxLength(25)]
		public string FirstName { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(2), MaxLength(25)]
		public string LastName { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(5), MaxLength(50)]
		public string Address { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(2), MaxLength(50)]
		public string City { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(4), MaxLength(8)]
		public string Postal { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(2), MaxLength(25)]
		public string Country { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(8), MaxLength(16)]
		public string Phone { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(6), MaxLength(50)]
		public string Email { get; set; }
	}
}
