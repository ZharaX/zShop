using System.Linq;

namespace Service.Concrete
{
	public interface IProduct
	{
		System.Linq.IQueryable<Data.Models.Product> GetProducts();
		bool Create(Data.Models.Product product);
		Data.Models.Product Retrieve(int id);
		bool Update(Data.Models.Product product);
		bool Delete(Data.Models.Product product);
	}

	public class ProductService : IProduct
	{
		private Data.DBManager.IDBManager<Data.Models.Product> productHandler;

		public ProductService(Data.DBManager.IDBManager<Data.Models.Product> product) { productHandler = product; }

		public IQueryable<Data.Models.Product> GetProducts()
		{
			return productHandler.Table;
		}
		public bool Create(Data.Models.Product product) { return productHandler.Create(product); }

		public Data.Models.Product Retrieve(int id) { return productHandler.Retrieve(id); }

		public bool Update(Data.Models.Product product) { return productHandler.Update(product); }

		public bool Delete(Data.Models.Product product) { return productHandler.Delete(product); }
	}
}
