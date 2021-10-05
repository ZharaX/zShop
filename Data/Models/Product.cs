namespace Data.Models
{
	/// <summary>
	/// Context Class Defining a Product
	/// </summary>
	public class Product
	{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Category Category { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public string Image { get; set; }
		public bool Active { get; set; }
	}
}
