using System.Collections.Generic;

namespace Data.Models
{
	/// <summary>
	/// Context Class Defining a Product
	/// NOTE: ANNOTATIONS ARE DEFINED BY FLUID API
	/// </summary>
	public class Product : BaseModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public string Image { get; set; }
		public bool Active { get; set; }

		// CATEGORY REFERENCE NAVIGATION
		public Category Category { get; set; }

		// ORDER COLLECTION NAVIGATION FOR THIS PRODUCT
		public ICollection<OrderProduct> Orders { get; set; }
	}
}
