using System.Collections.Generic;

namespace Service.Querys.Orders
{
	public class OrderFiltering
	{
		public List<DTO.OrderDTO> Orders { get; set; }
		public int TotalCount { get; set; }
	}
}
