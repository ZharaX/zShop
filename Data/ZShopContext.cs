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

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			//=> options.UseSqlServer(@"Server=S-HF-EUC-UB-04\ZZ_SQLSERVER;Database=BloggingDb;Trusted_Connection=True;")
			=> options.UseSqlServer(@"Server=ZZ-SERVER\ZZSQLSERVER;Database=BookStoreDb;Trusted_Connection=True;");
	}
}
