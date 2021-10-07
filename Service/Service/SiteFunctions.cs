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
		/// <param name="ownerID">Owner of Products</param>
		/// <returns></returns>
		object PerformAction<T>(ActionType action, FunctionName function, T data, int ownerID) where T : class;

		/// <summary>
		/// Method for Performing Site Wide Admin Functions
		/// </summary>
		/// <typeparam name="T">T for DATA</typeparam>
		/// <param name="action">CRUD Enum Selector</param>
		/// <param name="function">Action Enum Selector</param>
		/// <param name="data">T DATA</param>
		/// <param name="ownerID">Owner of Products</param>
		/// <returns></returns>
		object PerformAdminAction<T>(ActionType action, FunctionName function, T data, int ownerID) where T : class;

		/// <summary>
		/// Method for handling access to Site Repository Functions
		/// </summary>
		/// <returns>SiteRepository Class</returns>
		ISiteRepository SiteRepository();

		/// <summary>
		/// Method for handling access to Site Products Functions
		/// </summary>
		/// <returns>Data.Products Class</returns>
		Data.IProducts SiteProducts();
	}

	public class SiteFunctions : ISiteFunctions
	{
		// REFERENCE FOR DB MANAGER
		private Data.IDBManager _dbManager;

		// INTERFACE - CLASS REFERENCES
		ISiteRepository _siteRepository;
		Data.IProducts _siteProducts;

		/// <summary>
		/// Constructor: Setting class references on instantiation (Startup)
		/// </summary>
		/// <param name="connectionString">DB CONNECTION STRING</param>
		public SiteFunctions(string connectionString)
		{
			// CREATE DB MANAGER REFERENCE
			_dbManager = new Data.DBManager(connectionString);
			// PASSING 'IDBManager' REFERENCE
			_siteRepository = new SiteRepository(_dbManager);
			_siteProducts = new Data.Products(_dbManager);

			// SETTING DTO HANDLER CLAS REFERENCE
			DataTransferObjects.SiteRepository = _siteRepository;
		}

		public ISiteRepository SiteRepository() { return _siteRepository; }
		public Data.IProducts SiteProducts() { return _siteProducts; }

		#region NORMAL USER FUNCTION HANDLER
		/// <summary>
		/// Method handling retrieval only of various requested data.
		/// Create, Update & Delete will be an Admin only feature.
		/// Action: Retrieve
		/// Function: Location, Product etc.
		/// </summary>
		/// <param name="action">Enum Values</param>
		/// <param name="function">Enum Values</param>
		/// <param name="data">Data to be Handled</param>
		/// <param name="ownerID">Requesting Owner</param>
		/// <returns>Object: Depending on Data</returns>
		public object PerformAction<T>(ActionType action, FunctionName function, T data, int ownerID) where T : class
		{
			switch (action)
			{
				// CASE:
				// ACTION -> CREATE
				case ActionType.Create:
					return "NOT ALLOWED"; // NOT ADMIN -> CANNOT USE CREATE

				// CASE:
				// ACTION -> RETRIEVE
				case ActionType.Retrieve:
					if (function == FunctionName.StorageDataOwner) // FUNCTION: STORAGE
						return _siteRepository.AddUserForSession(data as string[]); /*.DBGetStorageOwnerData(data as string); // TODO:*/

					if (function == FunctionName.StorageForOwner) // FUNCTION: LOCATION
						return _dbManager.DBGetStorageForOwner(ownerID);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return ReturnProductData(data);

					if (function == FunctionName.Categorys) // FUNCTION: CATEGORY
						return _siteRepository.GetAllCategorys(ownerID);

					if (function == FunctionName.Containers) // FUNCTION: CONTAINERS
						return _dbManager.DBGetContainers(data as int[]);

					if (function == FunctionName.Colors) // FUNCTION: COLORS
						return _siteRepository.GetAllColors(ownerID);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> UPDATE
				case ActionType.Update:
					return "NOT ALLOWED"; // NOT ADMIN -> CANNOT USE UPDATE

				// CASE:
				// ACTION -> DELETE
				case ActionType.Delete:
					return "NOT ALLOWED"; // NOT ADMIN -> CANNOT USE DELETE


				default:
					return null; // DEFAULTS NULL IF WE EVEN SHOULD BE GETTING HERE!
			}
		}
		#endregion
		#region ADMIN USER FUNCTION HANDLER
		/// <summary>
		/// Method handling all Admin functionality
		/// Action: CRUD Model
		/// Function: Location, Product etc.
		/// </summary>
		/// <param name="action">Enum Values</param>
		/// <param name="function">Enum Values</param>
		/// <param name="data">Data to be Handled</param>
		/// <param name="ownerID">Requesting Owner</param>
		/// <returns>Object: Depending on Data</returns>
		public object PerformAdminAction<T>(ActionType action, FunctionName function, T data, int ownerID) where T : class
		{
			switch (action)
			{
				// CASE:
				// ACTION -> CREATE
				case ActionType.Create:
					if (function == FunctionName.StorageDataOwner) // FUNCTION: NEW STORAGE OWNER
					{
						Data.Storage.StorageData owner = DataTransferObjects.Storage(data);
						return _dbManager.DBCreateNewOwner(owner);
					}

					if (function == FunctionName.Location) // FUNCTION: LOCATION
						return ""; // TODO: LATER FEATURE CREATION

					if (function == FunctionName.Product) // FUNCTION: CREATE NEW PRODUCT
						return NewProduct(data, ownerID);

					if (function == FunctionName.Containers) // FUNCTION: CREATE NEW CONTAINER FOR ASSET
						return _dbManager.DBCreateContainer(data as int[], ownerID);

					if (function == FunctionName.NewLocationAsset) // FUNCTION: CREATE NEW ASSET FOR OWNER
						return CreateAssets(data as int[], ownerID);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> RETRIEVE
				case ActionType.Retrieve:
					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> UPDATE
				case ActionType.Update:
					if (function == FunctionName.ProductLocation) // FUNCTION: LOCATION
						return UpdateProductLocation(data);

					if (function == FunctionName.Product) // FUNCTION: PRODUCT
						return _dbManager.DBUpdateProduct(DataTransferObjects.Product(data), ownerID);

					return null; // WE SHOULD EVEN NOT BE GETTING HERE

				// CASE:
				// ACTION -> DELETE
				case ActionType.Delete:
					return null; // WE SHOULD EVEN NOT BE GETTING HERE


				default:
					return null; // DEFAULTS NULL IF WE EVEN SHOULD BE GETTING HERE!
			}
		}
		#endregion
		#region PRIVATE METHODS FOR SITE ACTIONS
		/// <summary>
		/// Method for handling the creation of new Assets (Rooms, Shelves, Pallets...)
		/// Also handles the correct DB Function based on incoming data
		/// </summary>
		/// <param name="assetData">The Assets Data</param>
		/// <param name="ownerID">Owner of Asset</param>
		/// <returns>DB Message</returns>
		private string CreateAssets(int[] assetData, int ownerID)
		{
			if (assetData[0] == -1) // DIFFERENT CREATE METHOD
				return _dbManager.DBCreateNewRoomAsset(assetData, ownerID);

			// ALL OTHER ASSETS ARE HANDLED THE SAME
			return _dbManager.DBCreateNewAsset(assetData, ownerID);
		}

		/// <summary>
		/// Method Product Creator and Database Inserter
		/// </summary>
		/// <param name="product">The Product Data Object(UI)</param>
		/// <param name="ownerID">Owner of Product</param>
		/// <returns>DB Message</returns>
		private string NewProduct<T>(T product, int ownerID) where T : class
		{
			// PRODUCT UI MODEL (UI) WILL BE 'CONVERTED' TO DATA.ProductData (DATA)
			Data.Product.ProductData p = DataTransferObjects.Product(product);

			// PERFORMS PRODUCT CREATION -> RETURNS DB MESSAGE
			return _siteProducts.AddNewProduct(p, _siteRepository.GetAllCategorys(ownerID).FindIndex(cID => cID.Contains(p.Category)), ownerID);
		}

		/// <summary>
		/// Method for finding Product per Barcode
		/// </summary>
		/// <param name="data">Barcode Sent</param>
		/// <returns>Data.Product (or Not Found String)</returns>
		private object ReturnProductData<T>(T data)
		{
			Data.Product.ProductData p = _dbManager.DBGetProductByBarcode(data.ToString(), 1);

			if (p != null)
			{
				if (p.ID == 0 && p.Barcode == "SERIOUSLY?!")
					return "ERROR!";
				else
					return p;
			}
			else
				return "Vare Ikke Fundet!";
		}

		/// <summary>
		/// Method for Updating a Products Location
		/// </summary>
		/// <param name="data">data[0]: PID -> data[1]: LocationData</param>
		/// <returns>Success/Fail Message</returns>
		private string UpdateProductLocation<T>(T data)
		{
			// T 'data' IS OF TYPE ARRAY SO WE CAST IT AS SUCH
			string[] t = data as string[];
			// ID:0 -> PRODUCT ID
			// ID:1 -> LOCATION DATA
			return _dbManager.DBSetProductLocation(t[0].ToString(), t[1].ToString());
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
		StorageDataOwner,
		StorageForOwner,
		Location,
		Product,
		ProductLocation,
		Categorys,
		Colors,
		Containers,
		NewLocationAsset
	}
}
