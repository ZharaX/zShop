using System;

namespace Data.Models
{
	/// <summary>
	/// Context Class Defining Order Details
	/// </summary>
	public class OrderDetails
	{
		public int OrderID { get; set; }
		public int CustomerID { get; set; }
		public DateTime Date { get; set; }
		public DateTime CompletedOn { get; set; }
		public bool IsProcessed { get; set; }
	}
}
