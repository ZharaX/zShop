using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
	public interface IZShopContext
	{
		ZShopContext ZShopDBContext();
	}

	/// <summary>
	/// Main DB Contact Class
	/// DONT USE THIS DIRECTLY! IMPLEMENT IDBMANAGER
	/// </summary>
	public class ZShopContext : DbContext, IZShopContext
	{
		public DbSet<Models.Customer> Customers { get; set; }
		public DbSet<Models.Product> Products { get; set; }
		public DbSet<Models.Order> Orders { get; set; }
		public DbSet<Models.OrderProduct> OrderProducts { get; set; }

		public ZShopContext(DbContextOptions options) : base(options) { }

		public ZShopContext ZShopDBContext() { return this; }

		#region MODEL CREATION
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// DEFINING ORDERPRODUCT JOIN TABLE REQUIREMENTS
			modelBuilder.Entity<Models.OrderProduct>().HasKey(op => new { op.OrderID, op.ProductID });
			modelBuilder.Entity<Models.OrderProduct>().HasOne(op => op.Order).WithMany(p => p.Products).HasForeignKey(o => o.OrderID);
			modelBuilder.Entity<Models.OrderProduct>().HasOne(op => op.Product).WithMany(o => o.Orders).HasForeignKey(o => o.ProductID);
			modelBuilder.Entity<Models.OrderProduct>().Property(o => o.ProductAmount).IsRequired();
			modelBuilder.Entity<Models.OrderProduct>().ToTable("OrderProduct");

			// DEFINING PRODUCT REQUIREMENTS
			modelBuilder.Entity<Models.Product>().HasKey("ID");
			//modelBuilder.Entity<Models.Product>().HasMany(p => p.Orders).WithMany(o => o.Products);
			modelBuilder.Entity<Models.Product>().Property(p => p.Name).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Product>().Property(p => p.Description).IsRequired().HasMaxLength(250);
			modelBuilder.Entity<Models.Product>().Property(p => p.Image).IsRequired().HasMaxLength(50);
			modelBuilder.Entity<Models.Product>().Property(p => p.Price).HasPrecision(8, 2);
			modelBuilder.Entity<Models.Product>().ToTable("Products");

			// DEFINING ORDER REQUIREMENTS
			modelBuilder.Entity<Models.Order>().HasKey("ID");
			//modelBuilder.Entity<Models.Order>().HasMany(o => o.Products).WithMany(p => p.Orders);
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
					Stock = 200,
					Image = "2.png",
					Active = true
				},
				new Models.Product
				{
					ID = 3,
					Name = "Test 3",
					Description = "Test Description 3",
					Price = 99.00m,
					Stock = 300,
					Image = "3.png",
					Active = true
				},
				new Models.Product
				{
					ID = 4,
					Name = "Test 4",
					Description = "Test Description 4",
					Price = 129.00m,
					Stock = 400,
					Image = "4.png",
					Active = true
				},
				new Models.Product
				{
					ID = 5,
					Name = "Test 5",
					Description = "Test Description 5",
					Price = 149.00m,
					Stock = 500,
					Image = "5.png",
					Active = true
				},
				new Models.Product
				{
					ID = 6,
					Name = "Test 6",
					Description = "Test Description 6",
					Price = 199.00m,
					Stock = 600,
					Image = "6.png",
					Active = true
				},
				new Models.Product
				{
					ID = 7,
					Name = "Test 7",
					Description = "Test Description 7",
					Price = 249.00m,
					Stock = 700,
					Image = "7.png",
					Active = true
				},
				new Models.Product
				{
					ID = 8,
					Name = "Test 8",
					Description = "Test Description 8",
					Price = 299.00m,
					Stock = 800,
					Image = "8.png",
					Active = true
				},
				new Models.Product
				{
					ID = 9,
					Name = "Test 9",
					Description = "Test Description 9",
					Price = 399.00m,
					Stock = 900,
					Image = "9.png",
					Active = true
				},
				new Models.Product
				{
					ID = 10,
					Name = "Test 10",
					Description = "Test Description 10",
					Price = 599.00m,
					Stock = 1000,
					Image = "10.png",
					Active = true
				},
				new Models.Product
				{
					ID = 11,
					Name = "Test 11",
					Description = "Test Description 11",
					Price = 2.50m,
					Stock = 1100,
					Image = "11.png",
					Active = true
				},

				new Models.Product
				{
					ID = 12,
					Name = "Test 12",
					Description = "Test Description 12",
					Price = 5.00m,
					Stock = 1200,
					Image = "12.png",
					Active = true
				},
				new Models.Product
				{
					ID = 13,
					Name = "Test 13",
					Description = "Test Description 13",
					Price = 7.50m,
					Stock = 1300,
					Image = "13.png",
					Active = true
				},
				new Models.Product
				{
					ID = 14,
					Name = "Test 14",
					Description = "Test Description 14",
					Price = 10.00m,
					Stock = 1400,
					Image = "14.png",
					Active = true
				},
				new Models.Product
				{
					ID = 15,
					Name = "Test 15",
					Description = "Test Description 15",
					Price = 50.00m,
					Stock = 1500,
					Image = "15.png",
					Active = true
				},
				new Models.Product
				{
					ID = 16,
					Name = "Test 16",
					Description = "Test Description 16",
					Price = 5.00m,
					Stock = 1600,
					Image = "16.png",
					Active = true
				},
				new Models.Product
				{
					ID = 17,
					Name = "Test 17",
					Description = "Test Description 17",
					Price = 7.50m,
					Stock = 1700,
					Image = "17.png",
					Active = true
				},
				new Models.Product
				{
					ID = 18,
					Name = "Test 18",
					Description = "Test Description 18",
					Price = 18.00m,
					Stock = 1800,
					Image = "18.png",
					Active = true
				},
				new Models.Product
				{
					ID = 19,
					Name = "Test 19",
					Description = "Test Description 19",
					Price = 1.00m,
					Stock = 1900,
					Image = "19.png",
					Active = true
				},
				new Models.Product
				{
					ID = 20,
					Name = "Test 20",
					Description = "Test Description 20",
					Price = 2.00m,
					Stock = 2000,
					Image = "20.png",
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

		public Models.Customer LoginCustomer(string[] cred) { return Customers.Find(1); } // TODO: THIS IS NO GOOD, BUT NESSACERY SINCE WE HAVE NO USERS TABLE YET
	}
}
