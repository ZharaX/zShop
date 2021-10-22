using System.Collections.Generic;

namespace zShopWeb.Models
{
	public class NewOrder
	{
		public List<int> OrderProducts { get; set; }
		public decimal OrderTotalPrice { get; set; }
	}
}
