using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
	/// <summary>
	/// Context Class Defining a Customer
	/// NOTE: ANNOTATIONS ARE DEFINED BY FLUID API
	/// </summary>
	public class Customer
	{
		public int CustomerID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Postal { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		
		// COLLECTION NAVIGATION
		public ICollection<Order> Orders { get; set; }
	}
}
