using System.Linq;

namespace Service.Concrete
{
	public interface IOrder
	{
		IQueryable<Data.Models.Order> GetOrders();
		bool Create(Data.Models.Order order);
		Data.Models.Order Retrieve(int id);
		bool Update(Data.Models.Order order);
		bool Delete(Data.Models.Order order);
	}

	public class OrderService : IOrder
	{
		private Data.DBManager.IDBManager<Data.Models.Order> orderHandler;

		public OrderService(Data.DBManager.IDBManager<Data.Models.Order> order) { orderHandler = order; }

		public IQueryable<Data.Models.Order> GetOrders()
		{
			return orderHandler.Table;
		}
		public bool Create(Data.Models.Order order) { return orderHandler.Create(order); }

		public Data.Models.Order Retrieve(int id) { return orderHandler.Retrieve(id); }

		public bool Update(Data.Models.Order order) { return orderHandler.Update(order); }

		public bool Delete(Data.Models.Order order) { return orderHandler.Delete(order); }
	}
}
