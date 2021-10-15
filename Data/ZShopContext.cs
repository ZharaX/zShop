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
			modelBuilder.Entity<Models.Product>().HasKey("ID");
			modelBuilder.Entity<Models.Product>().HasMany(p => p.Orders).WithMany(o => o.Products);
			modelBuilder.Entity<Models.Product>().Property(p => p.Name).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Product>().Property(p => p.Description).IsRequired().HasMaxLength(250);
			modelBuilder.Entity<Models.Product>().Property(p => p.Image).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Product>().Property(p => p.Price).HasPrecision(8, 2);
			modelBuilder.Entity<Models.Product>().ToTable("Products");

			// DEFINING ORDER REQUIREMENTS
			modelBuilder.Entity<Models.Order>().HasKey("ID");
			modelBuilder.Entity<Models.Order>().HasMany(o => o.Products).WithMany(p => p.Orders);
			modelBuilder.Entity<Models.Order>().Property(o => o.Amount).IsRequired();
			modelBuilder.Entity<Models.Order>().Property(o => o.TotalPrice).HasPrecision(8, 2);
			modelBuilder.Entity<Models.Order>().Property(o => o.Discount).HasPrecision(8, 2);
			modelBuilder.Entity<Models.Order>().Property(o => o.Date).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<Models.Order>().ToTable("Orders");

			// DEFINING CUSTOMER REQUIREMENTS
			modelBuilder.Entity<Models.Customer>().HasKey("ID");
			modelBuilder.Entity<Models.Customer>().HasMany(c => c.Orders).WithOne(o => o.Customer).HasForeignKey(o => o.CustomerID);
			modelBuilder.Entity<Models.Customer>().Property(c => c.FirstName).IsRequired().HasMaxLength(25);
			modelBuilder.Entity<Models.Customer>().Property(c => c.LastName).IsRequired().HasMaxLength(25);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Address).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Customer>().Property(c => c.City).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Postal).IsRequired().HasMaxLength(8);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Country).IsRequired().HasMaxLength(25);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Phone).IsRequired().HasMaxLength(16);
			modelBuilder.Entity<Models.Customer>().Property(c => c.Email).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Customer>().ToTable("Customers");

			#region SEEDING
			// CREATING SEEDED DATA
			modelBuilder.Entity<Models.Product>().HasData(
				new Models.Product
				{
					ID = 1,
					Name = "Test 1",
					Description = "Test Description 1",
					Price = 12.50m,
					Stock = 100,
					Image = "1.png",
					Active = true
				},

				new Models.Product
				{
					ID = 2,
					Name = "Test 2",
					Description = "Test Description 2",
					Price = 25.00m,
					Stock = 500,
					Image = "2.png",
					Active = true
				},
				new Models.Product
				{
					ID = 3,
					Name = "Test 3",
					Description = "Test Description 3",
					Price = 99.00m,
					Stock = 1000,
					Image = "3.png",
					Active = true
				}
			);

			modelBuilder.Entity<Models.Customer>().HasData(
				new Models.Customer
				{
					ID = 1,
					FirstName = "Jens",
					LastName = "Burmeister",
					Address = "Skovvej 22",
					City = "Sønderborg",
					Postal = "6400",
					Country = "Danmark",
					Phone = "31318859",
					Email = "jens114x@elevcampus.dk"
				}
			);
			#endregion
		}
		#endregion

		public new DbSet<TEntity> Set<TEntity>() where TEntity : Models.BaseModel
		{
			return base.Set<TEntity>(); // TODO: IS THIS NEEDED???
		}
	}
}
