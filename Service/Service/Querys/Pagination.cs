using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Querys
{
	/// <summary>
	/// Interface Orders Implementer
	/// </summary>
	public interface IFilterService
	{
		public Task<List<DTO.ProductDTO>> FilterProducts(string searchString, int currentPage, int pageSize, OrderBy order, ProductFilterBy filter);
		public Task<List<DTO.CustomerDTO>> FilterCustomers(string searchString, int currentPage, int pageSize, OrderBy order, CustomerFilterBy filter);
		public Task<List<DTO.OrderDTO>> FilterOrders(string searchString, int currentPage, int pageSize, OrderBy order, OrderFilterBy filter);
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
		public async Task<List<DTO.CustomerDTO>> FilterCustomers(string searchString, int currentPage, int pageSize, OrderBy order, CustomerFilterBy filter)
		{
			// THE QUERY!
			Customers.CustomerFiltering customerModel = new Customers.CustomerFiltering();
			var query = (List<DTO.CustomerDTO>)_siteFunctions.PerformAction(ActionType.Query, FunctionName.Customer, searchString);

			// TEST ORDER ATTACH TODO:
			foreach(DTO.CustomerDTO cust in query)
			{
				cust.Orders = (List<DTO.OrderDTO>)_siteFunctions.PerformAction(ActionType.Query, FunctionName.Order, cust);
			}

			// TOTAL PRODUCT COUNT
			customerModel.TotalCount = query.Count();

			// FILTER BY REFLECTION & ORDERBY
			var filterBy = filter.ToString();
			if (order == OrderBy.Ascending)
				customerModel.Customers = query.Skip((currentPage - 1) * pageSize).Take(pageSize).OrderBy(c => c.GetType().GetProperty(filterBy).GetValue(c, null)).ToList();

			if (order == OrderBy.Descending)
				customerModel.Customers = query.Skip((currentPage - 1) * pageSize).Take(pageSize).OrderByDescending(c => c.GetType().GetProperty(filterBy).GetValue(c, null)).ToList();

			return customerModel.Customers;
		}
		#endregion
		#region ORDER FILTERING
		/// <summary>
		/// Filters Orders by user settings.
		/// </summary>
		/// <param name="searchString">Name/Description Search</param>
		/// <param name="currentPage">The page we are currently on</param>
		/// <param name="pageSize">The amount of Orders to display</param>
		/// <param name="order">Ascending / Descending</param>
		/// <returns>List of Orders (Filtered / Ordered)</returns>
		public async Task<List<DTO.OrderDTO>> FilterOrders(string searchString, int currentPage, int pageSize, OrderBy order, OrderFilterBy filter)
		{
			// THE QUERY!
			Orders.OrderFiltering orderModel = new Orders.OrderFiltering();
			var query = (List<DTO.OrderDTO>)_siteFunctions.PerformAction(ActionType.Query, FunctionName.Order, searchString);

			// TOTAL PRODUCT COUNT
			orderModel.TotalCount = query.Count();

			// FILTER BY REFLECTION & ORDERBY
			var filterBy = filter.ToString();

			// FILTER BY REFLECTION & ORDERBY
			if (order == OrderBy.Ascending)
				orderModel.Orders = query.Skip((currentPage - 1) * pageSize).Take(pageSize).OrderBy(o => o.GetType().GetProperty(filterBy).GetValue(o, null)).ToList();

			if (order == OrderBy.Descending)
				orderModel.Orders = query.Skip((currentPage - 1) * pageSize).Take(pageSize).OrderByDescending(o => o.GetType().GetProperty(filterBy).GetValue(o, null)).ToList();

			return orderModel.Orders;
		}
		#endregion
	}

	public enum OrderBy { Ascending, Descending }

	public enum ProductFilterBy
	{
		Name,
		Price,
		InStock
	}

	public enum CustomerFilterBy
	{
		FirstName,
		LastName,
		Orders
	}

	public enum OrderFilterBy
	{
		Date,
		Price,
		IsCompleted
	}
}