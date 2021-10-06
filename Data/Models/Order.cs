using System;
using System.Collections.Generic;

namespace Data.Models
{
	/// <summary>
	/// Context Class Defining an Order
	/// </summary>
	public class Order
	{
		public int OrderID { get; set; }

		public int Amount { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal Discount { get; set; }

		public DateTime Date { get; set; }
		public DateTime CompletedOn { get; set; }
		public bool IsProcessed { get; set; }

		// CUSTOMER NAVIGATION REFERENCE
		public int CustomerID { get; set; }
		public Customer Customer { get; set; }

		// NAVIGATION COLLECTION OF PRODUCTS ON THIS ORDER
		public ICollection<Product> Products { get; set; }
	}
}
