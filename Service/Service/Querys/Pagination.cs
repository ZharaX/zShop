using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Service.Querys
{
	public interface IProductFilter
	{
		public List<DTO.ProductDTO> GetProductsContainingString(string searchString, int currentPage, int pageSize, OrderBy order, FilterBy filter);
	}

	public class ProductFilterService : IProductFilter
	{
		private readonly ISiteFunctions _siteFunctions;

		public ProductFilterService(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }


		public List<DTO.ProductDTO> GetProductsContainingString(string searchString, int currentPage, int pageSize, OrderBy order, FilterBy filter)
		{
			// THE QUERY!
			Products.ProductViewModel productModel = new Products.ProductViewModel();
			var query = (List<DTO.ProductDTO>)_siteFunctions.PerformAction(ActionType.Query, FunctionName.Product, searchString);

			// TOTAL PRODUCT COUNT
			productModel.TotalCount = query.Count();


			// FILTER BY REFLECTION & ORDERBY
			var filterBy = filter.ToString();
			if (order == OrderBy.Ascending)
				productModel.Products = query.Skip((currentPage - 1) * pageSize).Take(pageSize).OrderBy(p => p.GetType().GetProperty(filterBy).GetValue(p, null)).ToList();

			if (order == OrderBy.Descending)
				productModel.Products = query.Skip((currentPage - 1) * pageSize).Take(pageSize).OrderByDescending(p => p.GetType().GetProperty(filterBy).GetValue(p, null)).ToList();

			return productModel.Products;
		}
	}

	public enum OrderBy
	{
		Ascending,
		Descending
	}

	public enum FilterBy
	{
		Name,
		Price,
		InStock
	}
}