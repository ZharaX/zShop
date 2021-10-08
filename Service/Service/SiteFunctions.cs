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
		/// <typeparam name="T">T for DATA</typeparam>
		/// <param name="action">CRUD Enum Selector</param>
		/// <param name="function">Action Enum Selector</param>
		/// <param name="data">T DATA</param>
		/// <returns>Objects of Various Types</returns>
		object PerformAction<T>(ActionType action, FunctionName function, T data) where T : class;
	}

	/// <summary>
	/// SiteFunctions Class, one and only method for Site Wide Functionalitys
	/// <para>NOTE: DO NOT DIRECTLY USE THIS CLASS, IMPLEMENT ISiteFunctions</para>
	/// <see cref="ISiteFunctions"/>
	/// </summary>
	public class SiteFunctions : ISiteFunctions
	{
		// REFERENCE FOR DB MANAGER
		private readonly Data.IDBHandler _dbContext;
		private readonly ICustomerHandler _customerHandler;

		/// <summary>
		/// Constructor: Supplying ConnectionString for DBContext Use
		/// Instantiates: DataTransferObjects Class
		/// </summary>
		/// <param name="connectionString">DB CONNECTION STRING</param>
		public SiteFunctions(string connectionString)
		{
			// CREATE DB MANAGER REFERENCE
			_dbContext = new Data.ZShopContext();
			_customerHandler = new CustomerHandler(_dbContext);

			// SETTING DTO HANDLER CLASS REFERENCE
			//DataTransferObjects.SiteRepository = _siteRepository;
		}

		#region USER FUNCTION HANDLER
		/// <summary>
		/// Method handling retrieval only of various requested data.
		/// Create, Update & Delete will be an Admin only feature.
		/// Action: Retrieve
		/// Function: Customer, Order, Product etc.
		/// </summary>
		/// <param name="action">Enum Values</param>
		/// <param name="function">Enum Values</param>
		/// <param name="data">Data to be Handled</param>
		/// <returns>Object: Depending on Data</returns>
		public object PerformAction<T>(ActionType action, FunctionName function, T data) where T : class
		{
			switch (action)
			{
				// CASE:
				// ACTION -> CREATE
				case ActionType.Create:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return _dbContext.CreateCustomer(data as Data.Models.Customer);

					if (function == FunctionName.Order) // FUNCTION: PRODUCT
						return _dbContext.CreateOrder(data as Data.Models.Order);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return NewProduct(data);

					if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
						return _dbContext.CreateCategory(data as string);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> RETRIEVE
				case ActionType.Retrieve:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return _customerHandler.AddUserForSession(data as string[]);

					if (function == FunctionName.Order) // FUNCTION: ORDER
						return _dbContext.GetOrder(data as int[]);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return ReturnProductData(data);

					if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
						return _dbContext.GetAllCategorys();

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> UPDATE
				case ActionType.Update:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return _dbContext.UpdateCustomer(data as Data.Models.Customer);

					if (function == FunctionName.Order) // FUNCTION: ORDER
						return _dbContext.UpdateOrder(data as Data.Models.Order);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return UpdateProduct(data);

					if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
						return _dbContext.UpdateCategory(data as string);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> DELETE
				case ActionType.Delete:
					if (function == FunctionName.Customer) // FUNCTION: CUSTOMER
						return _dbContext.DeleteCustomer(data as int[]);

					if (function == FunctionName.Order) // FUNCTION: ORDER
						return _dbContext.DeleteOrder(data as int[]);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return _dbContext.DeleteProduct(data as int[]);

					if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
						return _dbContext.DeleteCategory(data as int[]);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE


				default:
					return null; // DEFAULTS NULL IF WE EVEN SHOULD BE GETTING HERE!
			}
		}
		#endregion
		#region PRIVATE METHODS FOR SITE ACTIONS
		/// <summary>
		/// Method Product Creator and Database Inserter
		/// </summary>
		/// <param name="data">The Product Data Object (UI)</param>
		/// <returns>Success/Failure Status</returns>
		private bool NewProduct<T>(T data)
		{
			// UI.PRODUCT WILL BE 'CONVERTED' TO DATA.MODELS.PRODUCT (DATA)
			//Data.Models.Product p = DataTransferObjects.Product(data);

			// PERFORMS PRODUCT CREATION -> RETURNS DB MESSAGE
			return false;
		}

		/// <summary>
		/// Method for finding Product per ID
		/// </summary>
		/// <param name="data">ID</param>
		/// <returns>Data.Models.Product or (Not Found String)</returns>
		private object ReturnProductData<T>(T data)
		{
			Data.Models.Product p = _dbContext.GetProduct(data as int[]);

			if (p != null) return p;
			
			return "Vare Ikke Fundet!";
		}

		/// <summary>
		/// Method for Updating a Product
		/// </summary>
		/// <param name="data">T Product Model</param>
		/// <returns>Success/Failure Status</returns>
		private bool UpdateProduct<T>(T data)
		{
			return _dbContext.UpdateProduct(data as Data.Models.Product);
		}
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
		Delete
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
