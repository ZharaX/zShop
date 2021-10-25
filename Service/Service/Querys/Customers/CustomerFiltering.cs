using System.Collections.Generic;

namespace Service.Querys.Customers
{
	public class CustomerFiltering
	{
		public List<DTO.CustomerDTO> Customers { get; set; }
		public int TotalCount { get; set; }
	}
}
