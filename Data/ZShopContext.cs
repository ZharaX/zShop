using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
	public interface IDBManager
	{
		ZShopContext DBManager();
	}

	/// <summary>
	/// Main DB Contact Class
	/// DONT USE THIS DIRECTLY! IMPLEMENT IDBMANAGER
	/// </summary>
	public class ZShopContext : DbContext, IDBManager
	{
		public DbSet<Models.Product> Products { get; set; }
		public DbSet<Models.Customer> Customers { get; set; }
		public DbSet<Models.Order> Orders { get; set; }

		public ZShopContext(DbContextOptions options) : base(options) { }

		public ZShopContext DBManager() { return this; }

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
				.HasForeignKey(x => x.ID);

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

		public new DbSet<TEntity> Set<TEntity>() where TEntity : Models.BaseModel
		{
			return base.Set<TEntity>();
		}
	}
}
