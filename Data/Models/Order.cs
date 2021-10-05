namespace Data.Models
{
	/// <summary>
	/// Context Class Defining an Order
	/// </summary>
	public class Order
	{
		public int OrderID { get; set; }
		public int ProductID { get; set; }
		public int Amount { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal Discount { get; set; }
	}
}
