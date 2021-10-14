using Data.DBManager;
using Data.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Service
{
	/// <summary>
	/// Site Functions Interface implementations
	/// <para>USE THIS TO ACCESS SITE FUNCTIONALITY</para>
	/// </summary>
	public interface ISiteFunctions
	{
		/// <summary>
		/// Method for Performing Site Wide User Functions
		/// </summary>
		/// <typeparam name="T">T Generic (Model / ID)</typeparam>
		/// <param name="action">CRUD Enum Selector</param>
		/// <param name="function">Function Enum Selector</param>
		/// <param name="data">T DATA</param>
		/// <returns>Objects of Various Types</returns>
		object PerformAction<T>(ActionType action, FunctionName function, T data);
	}

	/// <summary>
	/// SiteFunctions Class, one and only method for Site Wide Functionalitys
	/// <para>NOTE: DO NOT DIRECTLY USE THIS CLASS, IMPLEMENT ISiteFunctions</para>
	/// <see cref="ISiteFunctions"/>
	/// </summary>
	public class SiteFunctions : ISiteFunctions
	{
		#region CONTEXT DATA
		// ACCESS TO SERVICE LAYER AND FUNCTIONS
		private readonly Data.IDBManager _dbManager;
		#endregion

		// REFERENCES FOR MODEL DB CONTEXTS
		internal readonly IDBManager<Customer> _customerContext;
		internal readonly IDBManager<Order> _orderContext;
		internal readonly IDBManager<Product> _productContext;

		// REFERENCES FOR MODEL DB CONTEXTS
		internal readonly Concrete.ICustomer _customers;
		internal readonly Concrete.IOrder _orders;
		internal readonly Concrete.IProduct _products;

		//private readonly ICustomerHandler _customerHandler;

		/// <summary>
		/// Constructor: Supplying ConnectionString for DBContext
		/// Instantiates: DataTransferObjects Class
		/// Instantiates: Model Contexts
		/// </summary>
		public SiteFunctions(Data.IDBManager dbContext)
		{
			_dbManager = dbContext;

			// CREATING INDIVIDUAL MODEL DBCONTEXTS
			_customerContext = new DBManager<Customer>(_dbManager.DBManager());
			_orderContext = new DBManager<Order>(_dbManager.DBManager());
			_productContext = new DBManager<Product>(_dbManager.DBManager());

			// ATTACHING DBCONTEXT TO CONCRETE SERVICE TYPES
			_customers = new Concrete.CustomerService(_customerContext);
			_orders = new Concrete.OrderService(_orderContext);
			_products = new Concrete.ProductService(_productContext);

			// SETTING DTO HANDLER CLASS REFERENCE
			//DataTransferObjects.SiteRepository = _siteRepository;
		}

		#region FUNCTION HANDLER
		/// <summary>
		/// Method handling retrieval only of various requested data.
		/// Create, Update & Delete will be an Admin only feature.
		/// Action: Retrieve
		/// Function: Customer, Order, Product etc.
		/// </summary>
		/// <param name="action">Enum Values</param>
		/// <param name="function">Enum Values</param>
		/// <param name="data">T Generic (Model / ID)</param>
		/// <returns>Object: Depending on Data</returns>
		public object PerformAction<T>(ActionType action, FunctionName function, T data)
		{
			switch (action)
			{
				// CASE:
				// ACTION -> CREATE
				case ActionType.Create:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return _customers.Create(Querys.QueryManager.FromCustomerDTO(data as DTO.CustomerDTO));

					if (function == FunctionName.Order) // FUNCTION: PRODUCT
						return CreateOrder(data as DTO.OrderDTO);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return _products.Create(data as Product);

					//if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
					//	return _dbContext.CreateCategory(data as string);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> RETRIEVE
				case ActionType.Retrieve:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return Querys.QueryManager.ToCustomerDTO(_customers.GetCustomers().Where(c => c.ID == (int)(object)data)).FirstOrDefault();

					if (function == FunctionName.Order) // FUNCTION: ORDER
						return Querys.QueryManager.ToOrderDTO(_orders.GetOrders(), _customers.Retrieve((int)(object)data)).FirstOrDefault();

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return Querys.QueryManager.ToProductDTO(_products.GetProducts().Where(p => p.ID == (int)(object)data)).FirstOrDefault();

					//if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
					//	return _dbContext.GetAllCategorys();

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> UPDATE
				case ActionType.Update:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return _customers.Update(Querys.QueryManager.FromCustomerDTO(data as DTO.CustomerDTO));

					if (function == FunctionName.Order) // FUNCTION: ORDER
						return _orders.Update(data as Order);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return _products.Update(data as Product);

					//if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
					//	return _dbContext.UpdateCategory(data as string);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> DELETE
				case ActionType.Delete:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return _customers.Delete(data as Customer);

					if (function == FunctionName.Order) // FUNCTION: ORDER
						return _orders.Delete(data as Order);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return _products.Delete(data as Product);

				//	if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
				//		return _dbContext.DeleteCategory(data as int[]);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> RETRIEVE
				case ActionType.Query:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return Querys.QueryManager.ToCustomerDTO(_customers.GetCustomers()).ToList();

					if (function == FunctionName.Order) // FUNCTION: ORDER
						return Querys.QueryManager.ToOrderDTO(_orders.GetOrders(), null);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return Querys.QueryManager.ToProductDTO(_products.GetProducts()).ToList();

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				default:
					return null; // DEFAULTS NULL IF WE EVEN SHOULD BE GETTING HERE!
			}
		}
		#endregion
		#region CREATE ORDER
		private bool CreateOrder(DTO.OrderDTO order)
		{
			var products = _products.GetProducts().ToList();

			var newOrder = new Order
			{
				Amount = order.Amount,
				TotalPrice = order.TotalPrice,
				Discount = order.Discount,
				Date = System.DateTime.Now,
			};

			//var cust = _customers.GetCustomers().AsQueryable().Include(c => c.Orders).Single();

			//newOrder.Customer = cust;

			var op = new System.Collections.Generic.List<OrderProduct>();
			foreach (Service.DTO.ProductDTO p in order.Products)
			{
				op.Add(new OrderProduct { ProductID = p.ProductID/*, Order = newOrder*/ });
			}

			newOrder.Products = op;

			return _orders.Create(newOrder);
		}
		//newOrder.Customer = cust;

		//return _orders.Create(newOrder);
		#endregion
	}

	/// <summary>
	/// CRUD IMPLEMENTER ENUM
	/// </summary>
	public enum ActionType
	{
		Create,
		Retrieve,
		Update,
		Delete,
		Query
	}

	/// <summary>
	/// PERFORM ACTION ON WHAT FUNCTION
	/// </summary>
	public enum FunctionName
	{
		Customer,
		Order,
		Product,
		Categorys
	}
}
