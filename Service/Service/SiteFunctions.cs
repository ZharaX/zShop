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
		private readonly Data.IZShopContext _dbManager;
		private readonly ICustomerHandler _authUser;
		#endregion
		#region MODEL REFERENCES
		// REFERENCES FOR MODEL DB CONTEXTS
		internal readonly IDBManager<Customer> _customerContext;
		internal readonly IDBManager<Order> _orderContext;
		internal readonly IDBManager<Product> _productContext;

		// REFERENCES FOR MODEL DB CONTEXTS
		internal readonly Concrete.ICustomer _customers;
		internal readonly Concrete.IOrder _orders;
		internal readonly Concrete.IProduct _products;
		#endregion

		/// <summary>
		/// Constructor: Supplying ConnectionString for DBContext
		/// Instantiates: Models Services Contexts using same DBContext
		/// </summary>
		public SiteFunctions(Data.IZShopContext dbContext)
		{
			_dbManager = dbContext;
			_authUser = new CustomerHandler(this);

			#region DBCONTEXT SERVICES & CONCRETENESS
			// CREATING INDIVIDUAL MODEL DBCONTEXTS
			_customerContext = new DBManager<Customer>(_dbManager.ZShopDBContext());
			_orderContext = new DBManager<Order>(_dbManager.ZShopDBContext());
			_productContext = new DBManager<Product>(_dbManager.ZShopDBContext());

			// ATTACHING DBCONTEXT TO CONCRETE SERVICE TYPES
			_customers = new Concrete.CustomerService(_customerContext);
			_orders = new Concrete.OrderService(_orderContext);
			_products = new Concrete.ProductService(_productContext);
			#endregion
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
						return _products.Create(Querys.QueryManager.FromProductDTO(data as DTO.ProductDTO));

					//if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
					//	return _dbContext.CreateCategory(data as string);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> RETRIEVE
				case ActionType.Retrieve:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return _authUser.AddUserForSession(data as string[]);
					//return Querys.QueryManager.ToCustomerDTO(_customers.GetCustomers().Where(c => c.ID == (int)(object)data)).FirstOrDefault();

					if (function == FunctionName.Order) // FUNCTION: ORDER TODO: THE HARDCODED ID NEEDS TO BE OBTAINED FROM ADDUSERFORSESSION
						return Querys.QueryManager.ToOrdersDTO(_dbManager.ZShopDBContext().OrderProducts, _customers.Retrieve((int)(object)data)).ToList();

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return Querys.QueryManager.ToProductDTO(_products.GetProducts("").Where(p => p.ID == (int)(object)data));

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
						return _products.Update(Querys.QueryManager.FromProductDTO(data as DTO.ProductDTO));

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
						return _products.Delete(_products.Retrieve((int)(object)data));

				//	if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
				//		return _dbContext.DeleteCategory(data as int[]);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> RETRIEVE
				case ActionType.Query:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return Querys.QueryManager.ToCustomerDTO(_customers.GetCustomers(), null).ToList();

					if (function == FunctionName.Order) // FUNCTION: ORDER
						return Querys.QueryManager.ToOrdersDTO(_dbManager.ZShopDBContext().OrderProducts, _customers.Retrieve(1));

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return Querys.QueryManager.ToProductDTO(_products.GetProducts(data as string));

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> RETRIEVE
				case ActionType.Login:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER LOGIN
						return _dbManager.ZShopDBContext().LoginCustomer(data as string[]);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				default:
					return null; // DEFAULTS NULL IF WE EVEN SHOULD BE GETTING HERE!
			}
		}
		#endregion
		#region CREATE ORDER
		private bool CreateOrder(DTO.OrderDTO order)
		{
			// GET CUSTOMER AND EXISTING ORDERS
			var cust = _customers.GetCustomers().Where(c => c.ID == 1).Include(c => c.Orders).Single();
			cust.Orders = _orders.GetOrders().AsTracking().Where(o => o.Customer == cust).ToList();

			// CREATE THE NEW ORDER OBJECT
			var newOrder = new Data.Models.Order
			{
				Discount = order.Discount,
				Date = System.DateTime.Now,
				Customer = _dbManager.ZShopDBContext().Attach(cust).Entity,
			};

			// NEW JOIN TABLE LIST
			System.Collections.Generic.List<OrderProduct> op = new System.Collections.Generic.List<OrderProduct>();

			// LOOP PRODUCTS TO BE ORDERED
			foreach (Service.DTO.ProductDTO prod in order.Products)
			{
				// CHECKS IF THE STOCK AMOUNT EVEN HAVE THE AMOUNT REQUESTED, IF NOT RETURN FALSE (NO ORDER CREATE)
				if (_products.Retrieve(prod.ProductID).Stock - prod.Amount < 0) return false;
				else
					// GO AHEAD ADD PRODUCT TO ORDER
					op.Add(new OrderProduct { ProductID = prod.ProductID, ProductAmount = prod.Amount });
			}

			// ATTACH ORDERPRODUCT -> ADD TO NEW ORDER
			_dbManager.ZShopDBContext().AttachRange(op);
			newOrder.Products = op;

			// GO AHEAD CREATE NEW ORDER
			if (_orders.Create(newOrder))
			{
				// UPDATE PRODUCT STOCK WITH NEW VALUES
				foreach(OrderProduct op2 in newOrder.Products)
				{
					Product p = _products.Retrieve(op2.ProductID);

					p.Stock -= op2.ProductAmount;
					_products.Update(p);
				}

				return true; // DONE!
			}

			return false; // SOMETHING WENT WRONG!
		}
		#endregion
	}

	/// <summary>
	/// CRUD IMPLEMENTER ENUM + MODEL QUERYS & SEPARATE LOGIN FUNCTION
	/// </summary>
	public enum ActionType
	{
		Create,
		Retrieve,
		Update,
		Delete,
		Query,
		Login
	}

	/// <summary>
	/// PERFORM ACTION ON WHAT FUNCTIONALITY
	/// </summary>
	public enum FunctionName
	{
		Customer,
		Order,
		Product,
		Categorys
	}
}
