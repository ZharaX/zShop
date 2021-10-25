﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Querys
{
	/// <summary>
	/// Interface Product Implementer
	/// </summary>
	public interface IFilterService
	{
		public Task<List<DTO.ProductDTO>> FilterProducts(string searchString, int currentPage, int pageSize, OrderBy order, ProductFilterBy filter);
		public Task<List<DTO.CustomerDTO>> FilterCustomers(string searchString, int currentPage, int pageSize, OrderBy order, bool isCompleted);
	}

	public class FilterService : IFilterService
	{
		private readonly ISiteFunctions _siteFunctions;

		public FilterService(ISiteFunctions siteFunctions) { _siteFunctions = siteFunctions; }

		#region PRODUCT FILTERING
		/// <summary>
		/// Filters Products by user settings.
		/// </summary>
		/// <param name="searchString">Name/Description Search</param>
		/// <param name="currentPage">The page we are currently on</param>
		/// <param name="pageSize">The amount of Products to display</param>
		/// <param name="order">Ascending / Descending</param>
		/// <param name="filter">Filter on Property</param>
		/// <returns>List of Products (Filtered / Ordered)</returns>
		public async Task <List<DTO.ProductDTO>> FilterProducts(string searchString, int currentPage, int pageSize, OrderBy order, ProductFilterBy filter)
		{
			// THE QUERY!
			Products.ProductFiltering productModel = new Products.ProductFiltering();
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
		#endregion
		#region CUSTOMER FILTERING
		/// <summary>
		/// Filters Products by user settings.
		/// </summary>
		/// <param name="searchString">Name/Description Search</param>
		/// <param name="currentPage">The page we are currently on</param>
		/// <param name="pageSize">The amount of Products to display</param>
		/// <param name="order">Ascending / Descending</param>
		/// <param name="filter">Filter on Property</param>
		/// <returns>List of Products (Filtered / Ordered)</returns>
		public async Task<List<DTO.CustomerDTO>> FilterCustomers(string searchString, int currentPage, int pageSize, OrderBy order, bool isCompleted)
		{
			// THE QUERY!
			Customers.CustomerFiltering customerModel = new Customers.CustomerFiltering();
			var query = (List<DTO.CustomerDTO>)_siteFunctions.PerformAction(ActionType.Query, FunctionName.Customer, searchString);

			// TOTAL PRODUCT COUNT
			customerModel.TotalCount = query.Count();


			// FILTER BY REFLECTION & ORDERBY
			if (order == OrderBy.Ascending)
				customerModel.Customers = query.Skip((currentPage - 1) * pageSize).Take(pageSize).OrderBy(c => c.Orders.Select(c => c.IsProcessed == isCompleted)).ToList();

			if (order == OrderBy.Descending)
				customerModel.Customers = query.Skip((currentPage - 1) * pageSize).Take(pageSize).OrderByDescending(c => c.Orders.Select(c => c.IsProcessed == isCompleted)).ToList();

			return customerModel.Customers;
		}
		#endregion
	}

	public enum OrderBy
	{
		Ascending,
		Descending
	}

	public enum ProductFilterBy
	{
		Name,
		Price,
		InStock
	}
}