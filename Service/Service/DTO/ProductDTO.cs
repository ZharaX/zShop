﻿namespace Service.DTO
{
	/// <summary>
	/// DTO Product Class
	/// </summary>
	public class ProductDTO
	{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public string Image { get; set; }
		public bool Active { get; set; }
		public string Category { get; set; }
	}
}