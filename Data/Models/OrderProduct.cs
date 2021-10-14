using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public class OrderProduct
	{
		public int OrdersID { get; set; }

		public int ProductsID { get; set; }

		public Order Order { get; set; }
		public Product Product { get; set; }
	}
}
