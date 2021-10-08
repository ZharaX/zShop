using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
	/// <summary>
	/// INTERFACE IMPLEMENTATION
	/// Use this to implement DB Context Related Functionality
	/// </summary>
	public interface IDBHandler
	{
		/// <summary>
		/// Implementation of Customer DB Create
		/// </summary>
		/// <param name="customer">Customer Object</param>
		/// <returns>SUCCESS/FAILURE BOOL</returns>
		bool CreateCustomer(Models.Customer customer);

		/// <summary>
		/// Implementation of Retrieving Customer By ID
		/// </summary>
		/// <param name="id">Customer ID</param>
		/// <returns>Customer Object</returns>
		Models.Customer GetCustomer(int id);
		Models.Customer LoginCustomer(string[] cred);

		/// <summary>
		/// Implementation of Customer DB Update
		/// </summary>
		/// <param name="customer">Customer Object</param>
		/// <returns>SUCCESS/FAILURE BOOL</returns>
		bool UpdateCustomer(Models.Customer customer);


		/// <summary>
		/// Implementation of Product DB Create
		/// </summary>
		/// <param name="product">Product Object</param>
		/// <returns>SUCCESS/FAILURE BOOL</returns>
		bool CreateProduct(Models.Product product);

		/// <summary>
		/// Implementation of Retrieving Product By ID
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <returns>Product Object</returns>
		Models.Product GetProduct(int[] id);

		/// <summary>
		/// Implementation of Retrieving All Products
		/// </summary>
		/// <returns>List of Product Objects</returns>
		List<Models.Product> GetAllProducts();

		/// <summary>
		/// Implementation of Product DB Update
		/// </summary>
		/// <param name="product"></param>
		/// <returns>SUCCESS/FAILURE BOOL</returns>
		bool UpdateProduct(Models.Product product);


		/// <summary>
		/// Implementation of Order DB Create
		/// </summary>
		/// <param name="order">Order Objects</param>
		/// <returns>SUCCESS/FAILURE BOOL</returns>
		bool CreateOrder(Models.Order order);

		/// <summary>
		/// Implementation of Retrieving Order By ID
		/// </summary>
		/// <param name="id">Order ID</param>
		/// <returns>Order Object</returns>
		Models.Order GetOrder(int[] id);

		/// <summary>
		/// Implementation of Retrieving All Existing Orders
		/// </summary>
		/// <returns>List of Order Objects</returns>
		List<Models.Order> GetAllOrders();

		/// <summary>
		/// Implementation of Order DB Update
		/// </summary>
		/// <param name="order">Order Object</param>
		/// <returns>SUCCESS/FAILURE BOOL</returns>
		bool UpdateOrder(Models.Order order);

		List<Models.Category> GetAllCategorys();
	}

	/// <summary>
	/// Main DB Contact Class
	/// DONT USE THIS DIRECTLY! Implement IDBHandler
	/// <see cref="IDBHandler"/>
	/// </summary>
	public class ZShopContext : DbContext, IDBHandler
	{
		private readonly string _connectionString;

		public DbSet<Models.Product> Products { get; set; }
		public DbSet<Models.Customer> Customers { get; set; }
		public DbSet<Models.Order> Orders { get; set; }

		public ZShopContext() { } // USING EMPTY DEFAULT FOR NOW UNTIL DEPENDENCY IS SETUP FROM RAZOR
		public ZShopContext(string connectionString) { _connectionString = connectionString; } // LATER FOR DEPENDENCY INJECTION

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlServer(@"Server=S-HF-DB-666\ZZ_SQLSERVER;Database=zShopDB;Trusted_Connection=True;");
			//=> options.UseSqlServer(@"Server=ZZ-SERVER\ZZSQLSERVER;Database=zShopDB;Trusted_Connection=True;");

		#region MODEL CREATION
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// DEFINING PRODUCT REQUIREMENTS
			modelBuilder.Entity<Models.Product>().Property(x => x.Name)
				.IsRequired(true)
				.HasMaxLength(50);

			modelBuilder.Entity<Models.Product>().Property(x => x.Description)
				.IsRequired(true)
				.HasMaxLength(250);

			modelBuilder.Entity<Models.Product>().Property(x => x.Image)
				.IsRequired(true)
				.HasMaxLength(50);

			modelBuilder.Entity<Models.Product>().Property(x => x.Price)
				.HasPrecision(8, 2);

			// DEFINING ORDER REQUIREMENTS
			modelBuilder.Entity<Models.Order>().Property(x => x.Amount)
				.IsRequired(true);

			modelBuilder.Entity<Models.Order>().Property(x => x.TotalPrice)
				.HasPrecision(8, 2);

			modelBuilder.Entity<Models.Order>().Property(x => x.Discount)
				.HasPrecision(8, 2);

			modelBuilder.Entity<Models.Order>().Property(x => x.Date)
				.HasDefaultValueSql("GetDate()");

			// DEFINING CUSTOMER REQUIREMENTS
			modelBuilder.Entity<Models.Customer>() // MANY TO MANY
				.HasMany(x => x.Orders)
				.WithOne(x => x.Customer)
				.HasForeignKey(x => x.CustomerID);

			modelBuilder.Entity<Models.Customer>().Property(x => x.FirstName)
				.IsRequired(true)
				.HasMaxLength(25);

			modelBuilder.Entity<Models.Customer>().Property(x => x.LastName)
				.IsRequired(true)
				.HasMaxLength(25);

			modelBuilder.Entity<Models.Customer>().Property(x => x.Address)
				.IsRequired(true)
				.HasMaxLength(50);

			modelBuilder.Entity<Models.Customer>().Property(x => x.City)
				.IsRequired(true)
				.HasMaxLength(50);

			modelBuilder.Entity<Models.Customer>().Property(x => x.Postal)
				.IsRequired(true)
				.HasMaxLength(8);

			modelBuilder.Entity<Models.Customer>().Property(x => x.Country)
				.IsRequired(true)
				.HasMaxLength(25);

			modelBuilder.Entity<Models.Customer>().Property(x => x.Phone)
				.IsRequired(true)
				.HasMaxLength(16);

			modelBuilder.Entity<Models.Customer>().Property(x => x.Email)
				.IsRequired(true)
				.HasMaxLength(50);
		}
		#endregion
		#region CUSTOMER QUERYS
		// CUSTOMER LOGIN
		public Models.Customer LoginCustomer(string[] cred)
		{
			return null; // TODO:
		}

		// CREATE
		public bool CreateCustomer(Models.Customer customer)
		{
			Customers.Add(customer);
			
			if (SaveChanges() == 1) return true;

			return false;
		}

		// RETRIEVE
		public Models.Customer GetCustomer(int id)
		{
			return Customers.FirstOrDefault(c => c.CustomerID == id);
		}

		// UPDATE
		public bool UpdateCustomer(Models.Customer customer)
		{
			Update(customer);
			if(SaveChanges() == 1) return true;
			
			return false;
		}

		// DELETE (DEACTIVATE)
		#endregion
		#region PRODUCT QUERYS
		// CREATE
		public bool CreateProduct(Models.Product product)
		{
			Products.Add(product);
			if(SaveChanges() == 1) return true;

			return false;
		}

		// RETRIEVE 1
		public Models.Product GetProduct(int[] id)
		{
			return Products.FirstOrDefault(c => c.ProductID == id[0]);
		}

		// RETRIEVE ALL
		public List<Models.Product> GetAllProducts()
		{
			return Products.ToList();
		}

		// UPDATE
		public bool UpdateProduct(Models.Product product)
		{
			Update(product);
			if(SaveChanges() == 1) return true;

			return false;
		}

		// DELETE (DEACTIVATE)
		#endregion
		#region ORDER QUERYS
		// CREATE
		public bool CreateOrder(Models.Order order)
		{
			Orders.Add(order);
			if(SaveChanges() == 1) return true;

			return false;
		}

		// RETRIEVE 1
		public Models.Order GetOrder(int[] id)
		{
			return Orders.FirstOrDefault(c => c.OrderID == id[0]);
		}

		// RETRIEVE ALL
		public List<Models.Order> GetAllOrders()
		{
			return Orders.ToList();
		}

		// UPDATE
		public bool UpdateOrder(Models.Order order)
		{
			Update(order);
			if(SaveChanges() == 1) return true;

			return false;
		}

		// DELETE (DEACTIVATE)
		#endregion
		#region CATEGORY QUERY
		public List<Models.Category> GetAllCategorys()
		{
			return null;
		}
		#endregion
	}
}
