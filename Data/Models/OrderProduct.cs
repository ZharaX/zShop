namespace Data.Models
{
	public class OrderProduct
	{
		// PRIMARYS
		public int OrderID { get; set; }
		public int ProductID { get; set; }

		// ORDER DETAILS
		public int ProductAmount { get; set; }

		// NAVIGATION
		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }
	}
}
