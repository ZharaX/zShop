using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public class OrderProduct
	{
		public int OrderID { get; set; }

		public int ProductID { get; set; }

		public Order Order { get; set; }
		public Product Product { get; set; }
	}
}
