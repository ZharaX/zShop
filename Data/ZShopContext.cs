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
		/// <returns>DB Status String</returns>
		string CreateCustomer(Models.Customer customer);
		/// <summary>
		/// Implementation of Retrieving Customer By ID
		/// </summary>
		/// <param name="id">Customer ID</param>
		/// <returns>Customer Object</returns>
		Models.Customer GetCustomer(int id);
		/// <summary>
		/// Implementation of Customer DB Update
		/// </summary>
		/// <param name="customer">Customer Object</param>
		string UpdateCustomer(Models.Customer customer);

		/// <summary>
		/// Implementation of Product DB Create
		/// </summary>
		/// <param name="product">Product Object</param>
		/// <returns>DB Status String</returns>
		string CreateProduct(Models.Product product);
		/// <summary>
		/// Implementation of Retrieving Product By ID
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <returns>Product Object</returns>
		Models.Product GetProduct(int id);
		/// <summary>
		/// Implementation of Retrieving All Products
		/// </summary>
		/// <returns>List of Product Objects</returns>
		List<Models.Product> GetAllProducts();
		/// <summary>
		/// Implementation of Product DB Update
		/// </summary>
		/// <param name="product"></param>
		/// <returns>DB Status String</returns>
		string UpdateProduct(Models.Product product);

		/// <summary>
		/// Implementation of Order DB Create
		/// </summary>
		/// <param name="order">Order Objects</param>
		/// /// <returns>DB Status String</returns>
		string CreateOrder(Models.Order order);
		/// <summary>
		/// Implementation of Retrieving Order By ID
		/// </summary>
		/// <param name="id">Order ID</param>
		/// <returns>Order Object</returns>
		Models.Order GetOrder(int id);
		/// <summary>
		/// Implementation of Retrieving All Existing Orders
		/// </summary>
		/// <returns>List of Order Objects</returns>
		List<Models.Order> GetAllOrders();
		/// <summary>
		/// Implementation of Order DB Update
		/// </summary>
		/// <param name="order">Order Object</param>
		/// <returns>DB Status String</returns>
		string UpdateOrder(Models.Order order);
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
		// CREATE
		public string CreateCustomer(Models.Customer customer)
		{
			using (var dbContext = new ZShopContext())
			{
				dbContext.Customers.Add(customer);


				if (dbContext.SaveChanges() == 1)
					return "Kunde Oprettet!";
			};

			return "Ingen Kunde Oprettet!";
		}

		// RETRIEVE
		public Models.Customer GetCustomer(int id)
		{
			using (var dbContext = new ZShopContext())
			{
				return dbContext.Customers.FirstOrDefault(c => c.CustomerID == id);
			};
		}

		// UPDATE
		public string UpdateCustomer(Models.Customer customer)
		{
			using (var dbContext = new ZShopContext())
			{
				dbContext.Update(customer);
				dbContext.SaveChanges();
			};

			return null;
		}

		// DELETE (DEACTIVATE)
		#endregion
		#region PRODUCT QUERYS
		// CREATE
		public string CreateProduct(Models.Product product)
		{
			using (var dbContext = new ZShopContext())
			{
				dbContext.Products.Add(product);
				dbContext.SaveChanges();
			};

			return null;
		}

		// RETRIEVE 1
		public Models.Product GetProduct(int id)
		{
			using (var dbContext = new ZShopContext())
			{
				return dbContext.Products.FirstOrDefault(c => c.ProductID == id);
			};
		}

		// RETRIEVE ALL
		public List<Models.Product> GetAllProducts()
		{
			using (var dbContext = new ZShopContext())
			{
				return dbContext.Products.ToList();
			};
		}

		// UPDATE
		public string UpdateProduct(Models.Product product)
		{
			using (var dbContext = new ZShopContext())
			{
				dbContext.Update(product);
				dbContext.SaveChanges();
			};

			return null;
		}

		// DELETE (DEACTIVATE)
		#endregion
		#region ORDER QUERYS
		// CREATE
		public string CreateOrder(Models.Order order)
		{
			using (var dbContext = new ZShopContext())
			{
				dbContext.Orders.Add(order);
				dbContext.SaveChanges();
			};

			return null;
		}

		// RETRIEVE 1
		public Models.Order GetOrder(int id)
		{
			using (var dbContext = new ZShopContext())
			{
				return dbContext.Orders.FirstOrDefault(c => c.OrderID == id);
			};
		}

		// RETRIEVE ALL
		public List<Models.Order> GetAllOrders()
		{
			using (var dbContext = new ZShopContext())
			{
				return dbContext.Orders.ToList();
			};
		}

		// UPDATE
		public string UpdateOrder(Models.Order order)
		{
			using (var dbContext = new ZShopContext())
			{
				dbContext.Update(order);
				dbContext.SaveChanges();
			};

			return null;
		}

		// DELETE (DEACTIVATE)
		#endregion
	}
}
