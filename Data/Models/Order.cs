using System;
using System.Collections.Generic;

namespace Data.Models
{
	/// <summary>
	/// Context Class Defining an Order
	/// NOTE: ANNOTATIONS ARE DEFINED BY FLUID API
	/// </summary>
	public class Order : BaseModel
	{
		public decimal Discount { get; set; }

		public DateTime Date { get; set; }
		public DateTime CompletedOn { get; set; }
		public bool IsProcessed { get; set; }

		// CUSTOMER NAVIGATION REFERENCE
		public int CustomerID { get; set; }
		public Customer Customer { get; set; }

		// PRODUCTS COLLECTION NAVIGATION FOR THIS ORDER
		public virtual ICollection<OrderProduct> Products { get; set; }
	}
}
