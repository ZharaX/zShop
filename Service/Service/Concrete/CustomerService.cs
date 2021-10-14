using System.Linq;

namespace Service.Concrete
{
	public interface ICustomer
	{
		IQueryable<Data.Models.Customer> GetCustomers();
		bool Create(Data.Models.Customer customer);
		Data.Models.Customer Retrieve(int id);
		bool Update(Data.Models.Customer customer);
		bool Delete(Data.Models.Customer customer);
	}

	public class CustomerService : ICustomer
	{
		// DEPENDENCY REFERENCE
		private Data.DBManager.IDBManager<Data.Models.Customer> customerHandler;

		// ON INSTANTIATE -> SET DEPENDENCY
		public CustomerService(Data.DBManager.IDBManager<Data.Models.Customer> cust) { customerHandler = cust; }


		// QUERY CUSTOMER TABLE
		public IQueryable<Data.Models.Customer> GetCustomers() { return customerHandler.Table; }

		// CUSTOMER CRUD IMPLEMENTATIONS
		public bool Create(Data.Models.Customer customer) { return customerHandler.Create(customer); }

		public Data.Models.Customer Retrieve(int id) { return customerHandler.Retrieve(id); }

		public bool Update(Data.Models.Customer customer) { return customerHandler.Update(customer); }

		public bool Delete(Data.Models.Customer customer) { return customerHandler.Delete(customer); }
	}
}
