using System;
using System.Collections.Generic;

namespace Service.DTO
{
	/// <summary>
	/// DTO Product Class
	/// </summary>
	public class OrderDTO
	{
		public int OrderID { get; private set; }
		public int Amount { get; set; }
		public decimal TotalPrice { get; private set; }
		public decimal Discount { get; private set; }

		public DateTime Date { get; private set; }
		public DateTime CompletedOn { get; private set; }
		public bool IsProcessed { get; private set; }


		// CUSTOMER OWNING THIS ORDER
		public CustomerDTO Customer { get; set; }

		// PRODUCTS FOR THIS ORDER
		public List<ProductDTO> Products { get; set; }

		public OrderDTO() { }
		public OrderDTO(int oID, int amount, decimal discount, DateTime date, DateTime completed, bool isProcessed, List<ProductDTO> prod)
		{
			OrderID = oID;
			Amount = amount;
			Discount = discount;
			Date = date;
			CompletedOn = completed;
			IsProcessed = isProcessed;
			Products = prod;
		}

		public void CalculateTotalPrice()
		{
			foreach (ProductDTO p in Products) { TotalPrice += p.Price; }
		}
	}
}