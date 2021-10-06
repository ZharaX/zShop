using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
	public class ZShopContext : DbContext
	{
		public DbSet<Models.Product> Products { get; set; }
		public DbSet<Models.Customer> Customers { get; set; }
		public DbSet<Models.Order> Orders { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlServer(@"Server=S-HF-EUC-UB-04\ZZ_SQLSERVER;Database=zShopDB;Trusted_Connection=True;");
		//=> options.UseSqlServer(@"Server=ZZ-SERVER\ZZSQLSERVER;Database=BookStoreDb;Trusted_Connection=True;");

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// ADDING DECIMAL PRECISION TO BOOK PRICE
			modelBuilder.Entity<Models.Product>()
					.Property(x => x.Price)
					.HasPrecision(8, 2);

			// ADDING DECIMAL PRECISION TO PROMOTION PRICE
			modelBuilder.Entity<Models.Order>()
					.Property(x => x.TotalPrice)
					.HasPrecision(8, 2);

			// ADDING DECIMAL PRECISION TO PROMOTION PRICE
			modelBuilder.Entity<Models.Order>()
					.Property(x => x.Discount)
					.HasPrecision(8, 2);

			modelBuilder.Entity<Models.Customer>()
				.HasMany(x => x.Orders)
				.WithOne(x => x.Customer)
				.HasForeignKey(x => x.CustomerID);
		}
	}
}
