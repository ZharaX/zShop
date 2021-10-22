using System.Linq;

namespace Service.Concrete
{
	public interface IProduct
	{
		IQueryable<Data.Models.Product> GetProducts(string searchString);
		bool Create(Data.Models.Product product);
		Data.Models.Product Retrieve(int id);
		bool Update(Data.Models.Product product);
		bool Delete(Data.Models.Product product);
	}

	public class ProductService : IProduct
	{
		private Data.DBManager.IDBManager<Data.Models.Product> productHandler;

		public ProductService(Data.DBManager.IDBManager<Data.Models.Product> product) { productHandler = product; }

		public IQueryable<Data.Models.Product> GetProducts(string searchString)
		{
			var query = productHandler.Table;
			query = searchString != null ? query
				.Where(p => p.Name.ToLower()
				.Contains(searchString.ToLower()) || p.Description.ToLower()
				.Contains(searchString.ToLower()))
				.OrderBy(r => r.Name) : query;

			return query;
		}
		public bool Create(Data.Models.Product product) { return productHandler.Create(product); }

		public Data.Models.Product Retrieve(int id) { return productHandler.Retrieve(id); }

		public bool Update(Data.Models.Product product) { return productHandler.Update(product); }

		public bool Delete(Data.Models.Product product) { return productHandler.Delete(product); }
	}
}
