using Microsoft.EntityFrameworkCore;

namespace Data
{
	public interface IDBManager
	{
		ZShopContext DBManager();
	}

	/// <summary>
	/// Main DB Context Class
	/// DONT USE THIS DIRECTLY! IMPLEMENT IDBMANAGER
	/// </summary>
	public class ZShopContext : DbContext, IDBManager
	{
		public DbSet<Models.Product> Products { get; set; }
		public DbSet<Models.Customer> Customers { get; set; }
		public DbSet<Models.Order> Orders { get; set; }
		public DbSet<Models.OrderProduct> NewOrder { get; set; }

		public ZShopContext(DbContextOptions options) : base(options) { }

		public ZShopContext DBManager() { return this; }

		#region MODEL CREATION
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Models.OrderProduct>().HasKey(o => new { o.OrdersID, o.ProductsID });

			// CONFIGURING PRODUCT REQUIREMENTS
			modelBuilder.Entity<Models.Product>().HasKey(p => p.ID);
			modelBuilder.Entity<Models.Product>().Property(p => p.Name).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Product>().Property(p => p.Description).IsRequired().HasMaxLength(250);
			modelBuilder.Entity<Models.Product>().Property(p => p.Image).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Product>().Property(p => p.Price).IsRequired().HasPrecision(8, 2);
			modelBuilder.Entity<Models.Product>().ToTable("Products");

			// CONFIGURING ORDER REQUIREMENTS
			modelBuilder.Entity<Models.Order>().HasKey(o => o.ID);
			modelBuilder.Entity<Models.Order>().Property(o => o.Amount).IsRequired();
			modelBuilder.Entity<Models.Order>().Property(o => o.TotalPrice).HasPrecision(8, 2);
			modelBuilder.Entity<Models.Order>().Property(o => o.Discount).HasPrecision(8, 2);
			modelBuilder.Entity<Models.Order>().Property(o => o.Date).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<Models.Order>().ToTable("Orders");

			// CONFIGURING CUSTOMER REQUIREMENTS
			modelBuilder.Entity<Models.Customer>().HasKey(c => c.ID);
			modelBuilder.Entity<Models.Customer>().HasMany(c => c.Orders).WithOne(o => o.Customer).HasForeignKey(o => o.ID);
			modelBuilder.Entity<Models.Customer>().Property(c => c.FirstName).IsRequired().HasMaxLength(25);
			modelBuilder.Entity<Models.Customer>().Property(c => c.LastName).IsRequired().HasMaxLength(25);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Address).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Customer>().Property(c => c.City).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Postal).IsRequired().HasMaxLength(8);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Country).IsRequired().HasMaxLength(25);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Phone).IsRequired().HasMaxLength(16);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Email).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Customer>().ToTable("Customers");
		}
		#endregion

		public new DbSet<TEntity> Set<TEntity>() where TEntity : Models.BaseModel
		{
			return base.Set<TEntity>();
		}
	}
}
