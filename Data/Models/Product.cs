﻿using System.Collections.Generic;

namespace Data.Models
{
	/// <summary>
	/// Context Class Defining a Product
	/// NOTE: ANNOTATIONS ARE DEFINED BY FLUID API
	/// </summary>
	public class Product
	{
		public int ProductID { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(2), MaxLength(50)]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(5), MaxLength(250)]
		public string Description { get; set; }

		public decimal Price { get; set; }
		public int Stock { get; set; }

		[Required(AllowEmptyStrings = false), MinLength(5), MaxLength(50)]
		public string Image { get; set; }
		public bool Active { get; set; }

		// CATEGORY REFERENCE NAVIGATION
		public int CategoryID { get; set; }
		public Category Category { get; set; }

		// REFERENCE NAVIGATION
		public int CategoryID { get; set; }
		public Category Category { get; set; }

		// NAVIGATION COLLECTION OF PRODUCTS ON THIS ORDER
		public ICollection<Order> Orders { get; set; }
	}
}
