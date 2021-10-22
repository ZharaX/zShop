using System.Collections.Generic;

namespace Service.Querys.Products
{
	public class ProductViewModel
	{
		public List<DTO.ProductDTO> Products { get; set; }
		public int TotalCount { get; set; }
	}
}
